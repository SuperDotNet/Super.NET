using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Selection.Stores;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection;
using System;
using System.Reactive;

namespace Super.Runtime.Execution
{
	sealed class Contexts : ISelect<string, IDisposable>
	{
		public static Contexts Default { get; } = new Contexts();

		Contexts() : this(DisposeContext.Default, AssignedContext.Default, I<ContextDetails>.Default.From) {}

		readonly ICommand             _dispose;
		readonly IMutable<object>     _store;
		readonly Func<string, object> _context;

		public Contexts(ICommand dispose, IMutable<object> store, Func<string, object> context)
		{
			_dispose = dispose;
			_store   = store;
			_context = context;
		}

		public IDisposable Get(string parameter)
		{
			var current = _store.Get();
			var result = _dispose.And(_store.AsCommand()
			                                .Select()
			                                .Fix(current))
			                     .ToDelegate()
			                     .To(I<DelegatedDisposable>.Default);
			_store.Execute(_context(parameter));
			return result;
		}
	}

	sealed class DisposeContext : DecoratedCommand<Unit>, ICommand
	{
		public static DisposeContext Default { get; } = new DisposeContext();

		DisposeContext() : this(AssignedContext.Default.AsSpecification().Any(), AssociatedResources.Default) {}

		public DisposeContext(ISpecification<object> assigned, ISpecification<object, IDisposable> resources)
			: this(assigned.And(resources), resources.AsSelect()) {}

		public DisposeContext(ISpecification<object> specification, ISelect<object, IDisposable> select)
			: base(AssignedContext.Default
			                      .Enter(select.Enter(DisposeCommand.Default)
			                                   .And(AssignedContext.Default.Clear(), ClearResources.Default)
			                                   .Select()
			                                   .If(specification))) {}
	}

	sealed class ClearResources : RemoveCommand<object, Disposables>
	{
		public static ClearResources Default { get; } = new ClearResources();

		ClearResources() : base(AssociatedResources.Default) {}
	}

	sealed class AssociatedResources : AssociatedResource<object, Disposables>
	{
		public static AssociatedResources Default { get; } = new AssociatedResources();

		AssociatedResources() {}
	}
}
using Super.Model.Collections;
using Super.Model.Commands;
using Super.Model.Extents;
using Super.Model.Selection;
using Super.Model.Selection.Stores;
using Super.Model.Specifications;
using System;
using System.Reactive;

namespace Super.Runtime.Execution
{
	interface IContext : IMembership<IDisposable>, IDisposable
	{
		IContext Parent { get; }

		/*ContextDetails Details { get; }*/
	}

	sealed class DisposeContext : DecoratedCommand<Unit>
	{
		public static DisposeContext Default { get; } = new DisposeContext();

		DisposeContext() : this(AssignedContext.Default.Any(), AssociatedResources.Default) {}

		public DisposeContext(ISpecification<object> assigned, ISpecification<object, IDisposable> resources)
			: this(assigned, resources, resources) {}

		public DisposeContext(ISpecification<object> assigned, ISpecification<object> resources,
		                      ISelect<object, IDisposable> select)
			: base(DisposeCommand.Default
			                     .In(select)
			                     .And(AssignedContext.Default.Clear(), ClearResources.Default)
			                     .In()
			                     .In(resources.And(assigned))
			                     .In(ExecutionContext.Default)
			                     .Return()) {}
	}

	sealed class ClearResources : RemoveCommand<object, Disposables>
	{
		public static ClearResources Default { get; } = new ClearResources();

		ClearResources() : base(AssociatedResources.Default) {}
	}

	/*sealed class ContextResources : Contextual<IDisposable>
	{
		public static ContextResources Default { get; } = new ContextResources();

		ContextResources() : base(AssociatedResources.Default) {}
	}*/

	sealed class AssociatedResources : AssociatedResource<object, Disposables>
	{
		public static AssociatedResources Default { get; } = new AssociatedResources();

		AssociatedResources() {}
	}

	/*sealed class RootExecutionContext : ISource<IDisposable>
	{
		public static RootExecutionContext Default { get; } = new RootExecutionContext();

		RootExecutionContext() {}

		/*RootExecutionContext() : this(DomainUnload.Default.Get()) {}

		readonly IObservable<EventPattern<EventArgs>> _handler;

		public RootExecutionContext(IObservable<EventPattern<EventArgs>> handler) => _handler = handler;#1#

		public IDisposable Get()
		{
			var disposables = new Disposables();
			var result = new RootContext(disposables);
			/*_handler.Subscribe(result.Dispose);#1#
			return result;
		}
	}*/
}
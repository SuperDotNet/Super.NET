using Super.Compose;
using Super.Model.Commands;
using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Selection.Conditions;
using Super.Model.Selection.Stores;
using Super.Reflection;
using System;

namespace Super.Runtime.Execution
{
	sealed class Contexts : ISelect<string, IDisposable>
	{
		public static Contexts Default { get; } = new Contexts();

		Contexts() : this(DisposeContext.Default, ExecutionContextStore.Default, I<ContextDetails>.Default.From) {}

		readonly ICommand               _dispose;
		readonly IMutable<object>       _store;
		readonly Func<string, object>   _context;
		readonly Func<object, ICommand> _command;

		public Contexts(ICommand dispose, IMutable<object> store, Func<string, object> context)
			: this(dispose, store, context, store.AsCommand().Then().Out) {}

		// ReSharper disable once TooManyDependencies
		Contexts(ICommand dispose, IMutable<object> store, Func<string, object> context, Func<object, ICommand> command)
		{
			_dispose = dispose;
			_store   = store;
			_context = context;
			_command = command;
		}

		public IDisposable Get(string parameter)
		{
			var current = _store.Get();
			var result = _dispose.Then()
			                     .And(_command(current))
			                     .Selector()
			                     .To(I<Disposable>.Default);
			_store.Execute(_context(parameter));
			return result;
		}
	}

	sealed class DisposeContext : Command<None>, ICommand
	{
		public static DisposeContext Default { get; } = new DisposeContext();

		DisposeContext() : this(ExecutionContextStore.Default.Condition, AssociatedResources.Default) {}

		public DisposeContext(ICondition<None> assigned, IConditional<object, IDisposable> resources)
			: this(assigned, resources.Condition, resources) {}

		public DisposeContext(ICondition<None> assigned, ICondition<object> contains,
		                      ISelect<object, IDisposable> select)
			: base(select.Terminate(A.Of<DisposeCommand>())
			             .Then()
			             .And(ExecutionContextStore.Default.AsCommand().Then().Input().Any().Get(),
			                  A.Of<ClearResources>())
			             .To(x => x.Get().ToSelect())
			             .If(contains)
			             .To(x => x.ToCommand().Then())
			             .Input(ExecutionContextStore.Default)
			             .To(x => x.Get().ToSelect())
			             .If(assigned)
			             .ToAction()) {}
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
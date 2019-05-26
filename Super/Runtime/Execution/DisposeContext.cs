using System;
using Super.Compose;
using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Selection.Conditions;

namespace Super.Runtime.Execution
{
	sealed class DisposeContext : Command<None>, ICommand
	{
		public static DisposeContext Default { get; } = new DisposeContext();

		DisposeContext() : this(ExecutionContextStore.Default.Condition, AssociatedResources.Default) {}

		public DisposeContext(ICondition<None> assigned, IConditional<object, IDisposable> resources)
			: this(assigned, resources.Condition, resources) {}

		public DisposeContext(ICondition<None> assigned, ICondition<object> contains,
		                      ISelect<object, IDisposable> select)
			: base(select.Then()
			             .Terminate(A.Of<DisposeCommand>())
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
}
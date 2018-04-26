using Super.Model.Sources;
using Super.Runtime.Environment;

namespace Super.Runtime.Execution
{
	public sealed class ExecutionContext : DecoratedSource<object>, IExecutionContext
	{
		public static ExecutionContext Default { get; } = new ExecutionContext();

		ExecutionContext() : base(ExecutionContextStore.Default) {}
	}

	sealed class ExecutionContextStore : SystemAssignment<object>
	{
		public static ExecutionContextStore Default { get; } = new ExecutionContextStore();

		public ExecutionContextStore() : this(ComponentLocator<IExecutionContext>.Default) {}

		public ExecutionContextStore(ISource<ISource<object>> source)
			: base(source.Select(x => x.Value()
			                           .Or(() => new ContextDetails("Default Execution Context")))) {}
	}
}
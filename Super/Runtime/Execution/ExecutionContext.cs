using Super.Model.Results;

namespace Super.Runtime.Execution
{
	public sealed class ExecutionContext : Result<object>, IExecutionContext
	{
		public static ExecutionContext Default { get; } = new ExecutionContext();

		ExecutionContext() : base(ExecutionContextStore.Default) {}
	}
}
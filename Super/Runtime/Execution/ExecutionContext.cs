using Super.ExtensionMethods;
using Super.Model.Sources;

namespace Super.Runtime.Execution {
	sealed class ExecutionContext : DecoratedSource<object>, IExecutionContext
	{
		public static ExecutionContext Default { get; } = new ExecutionContext();

		ExecutionContext() : base(Implementations.Contexts.Select()) {}
	}
}
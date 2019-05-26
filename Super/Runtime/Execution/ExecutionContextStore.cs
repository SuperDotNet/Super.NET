using Super.Compose;
using Super.Model.Results;
using Super.Runtime.Environment;

namespace Super.Runtime.Execution
{
	sealed class ExecutionContextStore : SystemStore<object>
	{
		public static ExecutionContextStore Default { get; } = new ExecutionContextStore();

		ExecutionContextStore() : this(ExecutionContextLocator.Default) {}

		public ExecutionContextStore(IResult<IResult<object>> result)
			: base(Start.A.Result(() => new ContextDetails("Default Execution Context"))
			            .Start()
			            .Unless(result)
			            .Then()
			            .Value()
			            .Selector()) {}
	}
}
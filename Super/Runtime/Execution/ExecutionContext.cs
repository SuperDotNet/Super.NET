using Super.Compose;
using Super.Model.Results;
using Super.Reflection;
using Super.Runtime.Environment;

namespace Super.Runtime.Execution
{
	public sealed class ExecutionContext : DecoratedResult<object>, IExecutionContext
	{
		public static ExecutionContext Default { get; } = new ExecutionContext();

		ExecutionContext() : base(ExecutionContextStore.Default) {}
	}

	sealed class ExecutionContextLocator : DecoratedResult<IExecutionContext>
	{
		public static ExecutionContextLocator Default { get; } = new ExecutionContextLocator();

		ExecutionContextLocator() : base(A.This(ComponentTypesDefinition.Default)
		                                  .Select(x => x.Query().FirstAssigned().ToDelegate())
		                                  .Emit()
		                                  .To(I<ComponentLocator<IExecutionContext>>.Default)) {}
	}

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
			            .Out()) {}
	}
}
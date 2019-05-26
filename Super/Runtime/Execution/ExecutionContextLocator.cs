using Super.Compose;
using Super.Model.Results;
using Super.Reflection;
using Super.Runtime.Environment;

namespace Super.Runtime.Execution
{
	sealed class ExecutionContextLocator : Result<IExecutionContext>
	{
		public static ExecutionContextLocator Default { get; } = new ExecutionContextLocator();

		ExecutionContextLocator() : base(A.This(ComponentTypesDefinition.Default)
		                                  .Select(x => x.Query().FirstAssigned())
		                                  .Assume()
		                                  .To(I<ComponentLocator<IExecutionContext>>.Default)) {}
	}
}
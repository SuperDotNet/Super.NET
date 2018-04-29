using Super.Model.Commands;
using Super.Model.Sources;
using Super.Reflection;
using Super.Runtime.Environment;

namespace Super.Runtime.Execution
{
	public sealed class ExecutionContext : DecoratedSource<object>, IExecutionContext
	{
		public static ExecutionContext Default { get; } = new ExecutionContext();

		ExecutionContext() : base(ExecutionContextStore.Default) {}
	}

	public interface IConfiguration : ICommand<object>
	{

	}

	sealed class ExecutionContextLocator : DecoratedSource<IExecutionContext>
	{
		public static ExecutionContextLocator Default { get; } = new ExecutionContextLocator();

		ExecutionContextLocator() : base(ComponentTypesDefinition.Default
		                                                         .Select(x => x.FirstAssigned())
		                                                         .Emit()
		                                                         .To(I<ComponentLocator<IExecutionContext>>.Default)) {}
	}

	sealed class ExecutionContextStore : SystemStore<object>
	{
		public static ExecutionContextStore Default { get; } = new ExecutionContextStore();

		ExecutionContextStore() : this(ExecutionContextLocator.Default) {}

		public ExecutionContextStore(ISource<ISource<object>> source)
			: base(Start.New(() => new ContextDetails("Default Execution Context"))
			            .ToSource()
			            .Unless(source)
			            .AsSelect(x => x.Value())) {}
	}
}
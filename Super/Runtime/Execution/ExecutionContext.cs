using Super.Model.Sources;
using Super.Reflection;
using Super.Runtime.Environment;
using Super.Runtime.Invocation;

namespace Super.Runtime.Execution
{
	public sealed class ExecutionContext : Deferred<object>, IExecutionContext
	{
		public static ExecutionContext Default { get; } = new ExecutionContext();

		ExecutionContext() : this(ExecutionContextComponent.Default, AssignedContext.Default) {}

		public ExecutionContext(IExecutionContext source, IMutable<object> mutable) : base(source, mutable) {}
	}

	public sealed class ExecutionContextComponent : DecoratedSource<object>, IExecutionContext
	{
		public static ExecutionContextComponent Default { get; } = new ExecutionContextComponent();

		ExecutionContextComponent() : this(DefaultExecutionContext.Default) {}

		public ExecutionContextComponent(IExecutionContext @default)
			: base(@default.To(I<Component<IExecutionContext>>.Default)
			               .Out(x => x.Value())) {}
	}

	sealed class DefaultExecutionContext : DelegatedSource<object>, IExecutionContext
	{
		public static IExecutionContext Default { get; } = new DefaultExecutionContext();

		DefaultExecutionContext() : base(() => new ContextDetails("Default Execution Context")) {}
	}

	sealed class AssignedContext : Assignment<object>
	{
		public static AssignedContext Default { get; } = new AssignedContext();

		AssignedContext() : base(new Logical<object>()) {}
	}
}
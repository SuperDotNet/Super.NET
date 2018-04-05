using Super.Runtime.Activation;
using System.Collections.Generic;

namespace Super.Runtime
{
	sealed class ExecutionContexts : Ambient<Stack<IIExecutionContext>>
	{
		public static ExecutionContexts Default { get; } = new ExecutionContexts();

		ExecutionContexts() : base(New<ExecutionContextStack>.Default) {}
	}
}
using Super.Model.Selection;
using Super.Reflection;
using System;

namespace Super.Runtime.Execution
{
	sealed class ExecutionContexts : DecoratedSelect<string, IDisposable>, IExecutionContexts
	{
		public static ExecutionContexts Default { get; } = new ExecutionContexts();

		ExecutionContexts() : base(Implementations.Contexts.Select(I<ChildContexts>.Default).Select()) {}
	}
}
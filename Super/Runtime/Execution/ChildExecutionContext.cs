using System;
using Super.ExtensionMethods;
using Super.Model.Selection;
using Super.Reflection;

namespace Super.Runtime.Execution {
	sealed class ChildExecutionContext : Decorated<string, IDisposable>, IChildExecutionContext
	{
		public static ChildExecutionContext Default { get; } = new ChildExecutionContext();

		ChildExecutionContext() : base(Implementations.Contexts.Select(I<ChildContexts>.Default).Select()) {}
	}
}
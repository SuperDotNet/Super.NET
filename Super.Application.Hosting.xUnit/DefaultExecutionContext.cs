using Super.Model.Results;
using Super.Runtime.Execution;

// ReSharper disable All

namespace Super.Application.Hosting.xUnit
{
	public sealed class DefaultExecutionContext : Result<object>, IExecutionContext
	{
		public static IExecutionContext Default { get; } = new DefaultExecutionContext();

		DefaultExecutionContext() :
			base(() => new ContextDetails("xUnit Testing Application Default (root) Execution Context")) {}
	}
}
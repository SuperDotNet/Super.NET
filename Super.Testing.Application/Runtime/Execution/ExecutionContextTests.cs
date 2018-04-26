using FluentAssertions;
using Super.Runtime.Execution;
using Xunit;

namespace Super.Testing.Application.Runtime.Execution
{
	public sealed class ExecutionContextTests
	{
		readonly object _context;

		public ExecutionContextTests() : this(ExecutionContext.Default.Get()) {}

		ExecutionContextTests(object context) => _context = context;

		[Fact]
		void Verify()
		{
			var contexts = ExecutionContext.Default.Get();
			contexts.Should().BeSameAs(ExecutionContext.Default.Get());
			var current = ExecutionContext.Default.Get().To<ContextDetails>();
			ExecutionContext.Default.Get().Should().BeSameAs(current);

			ExecutionContext.Default.Get().Should().BeSameAs(_context);
		}
	}
}
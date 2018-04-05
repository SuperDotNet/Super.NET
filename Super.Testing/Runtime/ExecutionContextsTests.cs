using FluentAssertions;
using Super.Runtime;
using Xunit;
using Xunit.Abstractions;

namespace Super.Testing.Runtime
{
	public sealed class ExecutionContextsTests
	{
		readonly ITestOutputHelper _output;

		public ExecutionContextsTests(ITestOutputHelper output) => _output = output;

		[Fact]
		void Verify()
		{
			var stack = ExecutionContexts.Default.Get();
			stack.Should().BeSameAs(ExecutionContexts.Default.Get());
			var current = ExecutionContext.Default.Get();
			_output.WriteLine(current.Name);
			stack.Should().HaveCount(2);
			var reference = stack.Pop().Get();

			reference.Reference.Should().BeOfType<ObservedExecutionContextDetails>();

			reference.Should().BeSameAs(current);
			ExecutionContexts.Default.Get().Should().HaveCount(1);
		}
	}
}
using FluentAssertions;
using Super.Compose;
using Super.Model.Commands;
using Xunit;

namespace Super.Testing.Application.Compose.Commands
{
	public sealed class ContextTests
	{
		[Fact]
		void Verify()
		{
			Start.A.Command.Of.Any.By.Empty.Should().BeSameAs(EmptyCommand<object>.Default);
		}

		[Fact]
		void VerifyCalling()
		{
			var count = 0;
			Start.A.Command.Of.None.By.Calling(_ => count++).Execute();

			count.Should().Be(1);
		}
	}
}
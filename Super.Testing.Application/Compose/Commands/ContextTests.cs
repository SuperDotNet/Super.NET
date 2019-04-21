using FluentAssertions;
using Super.Model.Commands;
using Xunit;

namespace Super.Testing.Application.Compose.Commands
{
	public sealed class ContextTests
	{
		[Fact]
		void Verify()
		{
			Super.Compose.Start.A.Command.Of.Any.By.Empty.Should().BeSameAs(EmptyCommand<object>.Default);
		}

		[Fact]
		void VerifyCalling()
		{
			var count = 0;
			Super.Compose.Start.A.Command.Of.None.By.Calling(_ => count++).Execute();

			count.Should().Be(1);
		}
	}
}
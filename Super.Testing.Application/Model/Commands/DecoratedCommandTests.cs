using FluentAssertions;
using Super.Model.Commands;
using Super.Runtime;
using Xunit;

namespace Super.Testing.Application.Model.Commands
{
	public class DecoratedCommandTests
	{
		[Fact]
		public void Verify()
		{
			var count = 0;
			var inner = new Command<None>(x => count++);
			var sut   = new Command<None>(inner);
			sut.Execute();
			count.Should()
			     .Be(1);
		}
	}
}
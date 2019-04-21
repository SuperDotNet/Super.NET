using Super.Runtime;
using FluentAssertions;
using Super.Model.Commands;
using Xunit;

namespace Super.Testing.Application.Model.Commands
{
	public class DecoratedCommandTests
	{
		[Fact]
		public void Verify()
		{
			var count = 0;
			var inner = new DelegatedCommand<None>(x => count++);
			var sut   = new DecoratedCommand<None>(inner);
			sut.Execute();
			count.Should()
			     .Be(1);
		}
	}
}
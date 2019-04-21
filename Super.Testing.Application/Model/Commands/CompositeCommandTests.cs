using Super.Runtime;
using FluentAssertions;
using Super.Model.Commands;
using Xunit;

namespace Super.Testing.Application.Model.Commands
{
	public class CompositeCommandTests
	{
		[Fact]
		public void Verify()
		{
			var count = 0;
			new CompositeCommand<None>(new DelegatedCommand<None>(x => count++), new DelegatedCommand<None>(x => count++))
				.Execute();
			count.Should()
			     .Be(2);
		}
	}
}
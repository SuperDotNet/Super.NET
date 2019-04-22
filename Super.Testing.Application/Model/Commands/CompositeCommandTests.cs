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
			new CompositeCommand<None>(new Command<None>(x => count++), new Command<None>(x => count++))
				.Execute();
			count.Should()
			     .Be(2);
		}
	}
}
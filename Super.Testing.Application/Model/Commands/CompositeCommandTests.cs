using System.Reactive;
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
			new CompositeCommand<Unit>(new DelegatedCommand<Unit>(x => count++), new DelegatedCommand<Unit>(x => count++))
				.Execute();
			count.Should()
			     .Be(2);
		}
	}
}
using System.Linq;
using FluentAssertions;
using Super.Model.Sequences.Collections;
using Xunit;

namespace Super.Testing.Application.Model.Specifications
{
	public class HasTests
	{
		[Fact]
		public void Verify()
		{
			var item = new object();
			var items = item.Yield()
			                .ToList();
			new Has<object>(items).Get(item).Should().BeTrue();
		}
	}
}
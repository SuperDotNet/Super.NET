using System.Collections.Immutable;
using System.Linq;
using FluentAssertions;
using Super.Runtime;
using Xunit;

namespace Super.Testing.Reflection
{
	public class ItemsTests
	{
		[Fact]
		public void Coverage()
		{
			Empty<int>.Enumerable.Should()
			          .BeSameAs(Enumerable.Empty<int>());
			Empty<int>.Immutable.Should().BeEquivalentTo(ImmutableArray<int>.Empty);
		}
	}
}
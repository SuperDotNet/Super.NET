using FluentAssertions;
using Super.Runtime;
using System.Collections.Immutable;
using Xunit;

namespace Super.Testing.Application.Reflection
{
	public class ItemsTests
	{
		[Fact]
		public void Coverage()
		{
			Empty<int>.Enumerable.Should()
			          .BeSameAs(System.Linq.Enumerable.Empty<int>());
			Empty<int>.Immutable.Should().BeEquivalentTo(ImmutableArray<int>.Empty);
		}
	}
}
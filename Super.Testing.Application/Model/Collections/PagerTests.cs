using FluentAssertions;
using Super.Model.Collections;
using Xunit;

namespace Super.Testing.Application.Model.Collections
{
	public sealed class PagerTests
	{
		[Theory]
		[InlineData(1_000u)]
		[InlineData(1_025u)]
		[InlineData(10_000u)]
		[InlineData(100_000u)]
		void Verify(uint count)
		{
			var source = Objects.Count.Default.Get(count);
			var sut = Pager<int>.Default;
			sut.Get(source).Should().Equal(source);
		}
	}
}
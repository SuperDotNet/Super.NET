using FluentAssertions;
using Super.Model.Collections;
using System.Linq;
using Xunit;

namespace Super.Testing.Application.Model.Collections
{
	public sealed class EnumerateTests
	{
		[Theory]
		[InlineData(1u)]
		[InlineData(2u)]
		[InlineData(3u)]
		[InlineData(4u)]
		[InlineData(5u)]
		[InlineData(1_000u)]
		[InlineData(1_025u)]
		[InlineData(10_000u)]
		[InlineData(100_000u)]
		[InlineData(1_000_000u)]
		void Verify(uint count)
		{
			var source = Objects.Count.Default.Get(count);
			var sut = Enumerate<int>.Default;
			sut.Get(source.Cast<int>().GetEnumerator()).Result().Should().Equal(source);
		}
	}
}
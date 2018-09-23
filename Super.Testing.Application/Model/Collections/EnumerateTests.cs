using FluentAssertions;
using System.Collections.Generic;
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
			In<IEnumerable<int>>.Start().Iterate().Reference().Get(source).Should().Equal(source);
		}

		public uint Count { get; set; } = 10000;

		[Fact]
		void VerifyBasic()
		{
			In<IEnumerable<uint>>.Start()
			                     .Iterate()
			                     .Skip(500)
			                     .Take(100)
			                     .Reference()
			                     .Get(Numbers())
			                     .Should()
			                     .Equal(Numbers().Skip(500).Take(100));
		}

		IEnumerable<uint> Numbers()
		{
			for (var i = 0u; i < Count; i++)
			{
				yield return i;
			}
		}
	}
}
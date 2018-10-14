using FluentAssertions;
using System.Linq;
using Xunit;

namespace Super.Testing.Application.Model.Sequences
{
	public sealed class ArraySequenceTests
	{
		[Fact]
		void Verify()
		{
			var expected = Enumerable.Range(0, 10_000).ToArray();
			In<int[]>.Start()
			         .Sequence()
			         .Get()
			         .Get(expected)
			         .Should()
			         .Equal(expected);
		}

		[Fact]
		void VerifySelection()
		{
			var source   = Enumerable.Range(0, 10_000).ToArray();
			var expected = source.Skip(5000).Take(300).ToArray();
			In<int[]>.Start()
			         .Sequence()
			         .Skip(5000)
			         .Take(300)
			         .Get()
			         .Get(source)
			         .Should()
			         .Equal(expected);
		}
	}
}
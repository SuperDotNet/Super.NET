using FluentAssertions;
using Super.Model.Sequences;
using System.Linq;
using Xunit;

namespace Super.Testing.Application.Model.Sequences
{
	public sealed class ArrayQueryTests
	{
		[Fact]
		void Verify()
		{
			var expected = Enumerable.Range(0, 10_000).ToArray();
			var sut = In<int[]>.Start().Query().Get();
			var array = sut.Get(expected);
			array.Should().Equal(expected);
			array.Should().BeSameAs(expected);
		}

		[Fact]
		void VerifySelection()
		{
			var source = Enumerable.Range(0, 10_000).ToArray();
			var expected = source.Skip(5000).Take(300).ToArray();
			var sut      = In<int[]>.Start().Query().Skip(5000).Take(300).Get();
			var array    = sut.Get(source);
			array.Should().Equal(expected);
		}
	}
}
using FluentAssertions;
using System.Linq;
using Xunit;

namespace Super.Testing.Application.Model.Sequences
{
	public sealed class SequenceTests
	{
		[Fact]
		void Verify()
		{
			var array = new[] {1, 2, 3};

			Start.Sequence<int>()
			     .Get()
			     .Get(array)
			     .Should()
			     .Equal(array);
		}

		[Fact]
		void Skip()
		{
			var array    = new[] {1, 2, 3};
			var expected = array.Skip(1).ToArray();

			Start.Sequence<int>()
			     .Skip(1)
			     .Get()
			     .Get(array)
			     .Should()
			     .Equal(expected);
		}

		[Fact]
		void Take()
		{
			var array    = new[] {1, 2, 3};
			var expected = array.Take(2).ToArray();

			Start.Sequence<int>()
			     .Take(2)
			     .Get()
			     .Get(array)
			     .Should()
			     .Equal(expected);
		}

		[Fact]
		void SkipTake()
		{
			var array    = new[] {1, 2, 3, 4, 5};
			var expected = array.Skip(3).Take(2).ToArray();

			Start.Sequence<int>()
			     .Skip(3)
			     .Take(2)
			     .Get()
			     .Get(array)
			     .Should()
			     .Equal(expected);
		}
	}
}
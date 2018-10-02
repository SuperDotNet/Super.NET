using FluentAssertions;
using Super.Model.Sequences;
using System.Linq;
using Xunit;

namespace Super.Testing.Application.Model.Sequences
{
	public sealed class QueryTests
	{
		[Fact]
		void Verify()
		{
			var array = new[] {1, 2, 3};

			In<int[]>.Start()
			         .Query()
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

			In<int[]>.Start()
			         .Query()
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

			In<int[]>.Start()
			         .Query()
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

			In<int[]>.Start()
			         .Query()
			         .Skip(3)
			         .Take(2)
			         .Get()
			         .Get(array)
			         .Should()
			         .Equal(expected);
		}
	}
}
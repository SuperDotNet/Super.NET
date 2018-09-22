using FluentAssertions;
using System.Linq;
using Xunit;

namespace Super.Testing.Application.Model.Collections
{
	public class CompositionTests
	{
		[Fact]
		void Verify()
		{
			var array = new[] {1, 2, 3};

			var result = In<int[]>.Start().Iterate().Reference().Get(array);
			result.Should().Equal(array);
			result.Should().BeSameAs(array);
		}

		[Fact]
		void Skip()
		{
			var array = new[] {1, 2, 3};
			var expected = array.Skip(1).ToArray();

			var result = In<int[]>.Start().Iterate().Skip(1).Reference().Get(array);
			result.Should().Equal(expected);
			result.Should().NotBeSameAs(array);
		}

		[Fact]
		void Take()
		{
			var array    = new[] {1, 2, 3};
			var expected = array.Take(2).ToArray();

			var result = In<int[]>.Start().Iterate().Take(2).Reference().Get(array);
			result.Should().Equal(expected);
			result.Should().NotBeSameAs(array);
		}


		[Fact]
		void SkipTake()
		{
			var array    = new[] {1, 2, 3, 4, 5};
			var expected = array.Skip(3).Take(2).ToArray();

			var result = In<int[]>.Start().Iterate().Skip(3).Take(2).Reference().Get(array);
			result.Should().Equal(expected);
			result.Should().NotBeSameAs(array);
		}
	}
}
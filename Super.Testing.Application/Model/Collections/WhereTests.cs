// ReSharper disable ComplexConditionExpression

using FluentAssertions;
using System.Linq;
using Xunit;

namespace Super.Testing.Application.Model.Collections
{
	public sealed class WhereTests
	{
		[Fact]
		void VerifyWhereLink()
		{
			var numbers  = new[] {1, 2, 3, 4, 5};
			var expected = numbers.Where(x => x > 3).ToArray();
			In<int[]>.Start()
			         .Iterate()
			         .WhereBy(x => x > 3)
			         .Reference()
			         .Get(numbers)
			         .Should()
			         .Equal(expected);
		}

		[Fact]
		void VerifyCount()
		{
			var source   = Enumerable.Range(0, 10_000).ToArray();
			var expected = source.Where(x => x > 1000).ToArray();
			In<int[]>.Start()
			         .Iterate()
			         .WhereBy(x => x > 1000)
			         .Reference()
			         .Get(source)
			         .Should()
			         .Equal(expected);
		}

		/*[Fact]
		void VerifyWhereTake()
		{
			var numbers  = new[] {1, 2, 3, 4, 5};
			var expected = numbers.Where(x => x > 3).Take(1).ToArray();
			var actual   = In<int[]>.Start().Iterate().WhereAs(x => x > 3).Take(1).Reference().Get(numbers);
			actual.Should().Equal(expected);
		}*/

		[Fact]
		void Verify()
		{
			const uint count = 10_000_000u;
			var array = Objects.Count.Default
			                   .Iterate()
			                   .Skip(count - 5)
			                   .Take(5)
			                   .Result()
			                   .Get(count)
				//.Get()
				;
			array.Should().HaveCount(5);

			Objects.Count.Default.Get(count).Skip((int)(count - 5)).Take(5).Sum().Should().Be(array.Sum());

			/*var segment = Objects.Count.Default
			                          .Iterate()
			                          .Selection(x => x + 1)
			                          .Get(10_000);
			segment.Count.Should().Be(10_000);

			segment.ToArray().Sum(x => x).Should().Be(Objects.Count.Default.Get(10_000).Sum(x => x) + 10_000);*/
		}
	}
}
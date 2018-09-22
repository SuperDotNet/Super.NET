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
			var ints = In<int[]>.Start()
			                    .Iterate()
			                    .WhereBy(x => x > 3)
			                    .Reference()
			                    .Get(numbers);
			ints.Should().Equal(expected);
			ints.Should().NotBeSameAs(numbers);
		}

		[Fact]
		void VerifyCount()
		{
			var source   = Enumerable.Range(0, 10_000).ToArray();
			var expected = source.Where(x => x > 1000).ToArray();
			var ints = In<int[]>.Start()
			                    .Iterate()
			                    .WhereBy(x => x > 1000)
			                    .Reference()
			                    .Get(source);
			ints.Should().NotBeSameAs(source);
			ints.Should().Equal(expected);
			ints.Should().HaveCountGreaterThan(5000);
		}

		[Fact]
		void VerifyWhereTake()
		{
			var numbers  = new[] {1, 2, 3, 4, 5};
			var expected = numbers.Where(x => x > 3).Take(1).ToArray();
			var actual   = In<int[]>.Start().Iterate().WhereBy(x => x > 3).Take(1).Reference().Get(numbers);
			actual.Should().Equal(expected);
			actual.Should().NotBeSameAs(numbers);
		}

		[Fact]
		void Verify()
		{
			const uint count = 10_000_000u;
			var array = Objects.Count.Default
			                   .Iterate()
			                   .Skip(count - 5)
			                   .Take(5)
			                   .Reference()
			                   .Get(count);
			array.Should().HaveCount(5);

			Objects.Count.Default.Get(count).Skip((int)(count - 5)).Take(5).Sum().Should().Be(array.Sum());
		}

		[Fact]
		void VerifyWhereSkipTake()
		{
			var source = Enumerable.Range(0, 10_000).ToArray();
			var count = 8500;
			source
				.Where(x => x > 1000)
				.Skip(count)
				.Take(5)
				.Should()
				.Equal(In<int[]>.Start()
				                .Iterate()
				                .WhereBy(x => x > 1000)
				                .Skip((uint)count)
				                .Take(5)
				                .Reference()
				                .Get(source));
		}
	}
}
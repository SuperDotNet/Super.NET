using FluentAssertions;
using System.Linq;
using Xunit;

namespace Super.Testing.Application.Model.Sequences
{
	public sealed class CollectionSequenceTests
	{
		[Fact]
		void Verify()
		{
			var expected = Enumerable.Range(0, 10_000).ToArray();
			Start.List<int>()
			     .Sequence()
			     .Get()
			     .Get(expected.ToList())
			     .Should()
			     .Equal(expected);
		}

		[Fact]
		void VerifySelection()
		{
			var source   = Enumerable.Range(0, 10_000).ToArray();
			var expected = source.Skip(5000).Take(300).ToArray();
			Start.List<int>()
			     .Sequence()
			     .Skip(5000)
			     .Take(300)
			     .Get()
			     .Get(source.ToList())
			     .Should()
			     .Equal(expected);
		}

		[Fact]
		void VerifyWhereLink()
		{
			var numbers  = new[] {1, 2, 3, 4, 5};
			var expected = numbers.Where(x => x > 3).ToArray();
			Start.List<int>()
			     .Sequence()
			     .WhereBy(x => x > 3)
			     .Get()
			     .Get(numbers.ToList())
			     .Should()
			     .Equal(expected);
		}

		[Fact]
		void VerifyCount()
		{
			var source   = Enumerable.Range(0, 10_000).ToArray();
			var expected = source.Where(x => x > 1000).ToArray();
			var ints = Start.List<int>()
			                .Sequence()
			                .WhereBy(x => x > 1000)
			                .Get()
			                .Get(source.ToList());
			ints.Should().NotBeSameAs(source);
			ints.Should().Equal(expected);
			ints.Should().HaveCountGreaterThan(5000);
		}

		[Fact]
		void VerifyWhereTake()
		{
			var numbers  = new[] {1, 2, 3, 4, 5};
			var expected = numbers.Where(x => x > 3).Take(1).ToArray();
			var actual   = Start.List<int>().Sequence().WhereBy(x => x > 3).Take(1).Get().Get(numbers.ToList());
			actual.Should().Equal(expected);
			actual.Should().NotBeSameAs(numbers);
		}

		[Fact]
		void VerifyWhereSkipTake()
		{
			var source = Enumerable.Range(0, 10_000).ToArray();
			var count  = 8500;
			source.Where(x => x > 1000)
			      .Skip(count)
			      .Take(5)
			      .Should()
			      .Equal(Start.List<int>()
			                  .Sequence()
			                  .WhereBy(x => x > 1000)
			                  .Skip((uint)count)
			                  .Take(5)
			                  .Get()
			                  .Get(source.ToList()));
		}

		[Fact]
		void VerifyAdvanced()
		{
			var source = Enumerable.Range(0, 10_000).ToList();
			Start.List<int>()
			     .Sequence()
			     .Skip(3000)
			     .Take(1000)
			     .WhereBy(x => x > 1000)
			     .Get()
			     .Get(source)
			     .Should()
			     .Equal(source.Skip(3000)
			                  .Take(1000)
			                  .Where(x => x > 1000)
			                  .ToArray());
		}

		[Fact]
		void VerifyComprehensive()
		{
			var source = Enumerable.Range(0, 10_000).ToList();
			Start.List<int>()
			     .Sequence()
			     .Skip(3000)
			     .Take(2000)
			     .WhereBy(x => x > 1000)
			     .Skip(500)
			     .Take(1000)
			     .Get()
			     .Get(source)
			     .Should()
			     .Equal(source.Skip(3000)
			                  .Take(2000)
			                  .Where(x => x > 1000)
			                  .Skip(500)
			                  .Take(1000)
			                  .ToArray());
		}
	}
}
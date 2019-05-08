﻿using BenchmarkDotNet.Attributes;
using FluentAssertions;
using Super.Compose;
using Super.Model.Selection;
using Super.Testing.Objects;
using System;
using System.Linq;
using Xunit;

namespace Super.Testing.Application.Model.Sequences
{
	public sealed class WhereTests
	{
		[Fact]
		void VerifyWhereLink()
		{
			var numbers  = new[] {1, 2, 3, 4, 5};
			var expected = numbers.Where(x => x > 3).ToArray();
			Start.A.Selection<int>()
			     .As.Sequence.Array.By.Self.Query()
			     .WhereBy(x => x > 3)
			     .Out()
			     .Get(numbers)
			     .Should()
			     .Equal(expected);
		}

		[Fact]
		void VerifyCount()
		{
			var source   = Enumerable.Range(0, 1_000).ToArray();
			var expected = source.Where(x => x > 100).ToArray();
			var ints = Start.A.Selection<int>()
			                .As.Sequence.Array.By.Self.Query()
			                .WhereBy(x => x > 100)
			                .Out()
			                .Get(source);
			ints.Should().NotBeSameAs(source);
			ints.Should().Equal(expected);
			ints.Should().HaveCountGreaterThan(500);
		}

		[Fact]
		void Verify()
		{
			const uint count = 1_000u;
			var array = Numbers.Default.Open()
			                   .Query()
			                   .Skip(count - 5)
			                   .Take(5)
			                   .Out()
			                   .Get(count);
			array.Should().HaveCount(5);

			Numbers.Default.Get(count).Open().Skip((int)(count - 5)).Take(5).Sum().Should().Be(array.Sum());
		}

		[Fact]
		void VerifyWhereTake()
		{
			var numbers  = new[] {1, 2, 3, 4, 5};
			var expected = numbers.Where(x => x > 3).Take(1).ToArray();
			var actual = Start.A.Selection<int>()
			                  .As.Sequence.Array.By.Self.Query()
			                  .WhereBy(x => x > 3)
			                  .Take(1)
			                  .Out()
			                  .Get(numbers);
			actual.Should().Equal(expected);
			actual.Should().NotBeSameAs(numbers);
		}

		[Fact]
		void VerifyWhereSkipTake()
		{
			var source = Enumerable.Range(0, 1_000).ToArray();
			var count  = 850;
			source.Where(x => x > 100)
			      .Skip(count)
			      .Take(5)
			      .Should()
			      .Equal(Start.A.Selection<int>()
			                  .As.Sequence.Array.By.Self.Query()
			                  .WhereBy(x => x > 100)
			                  .Skip((uint)count)
			                  .Take(5)
			                  .Out()
			                  .Get(source));
		}

		[Fact]
		void VerifyAdvanced()
		{
			var source = Enumerable.Range(0, 1_000).ToArray();
			Start.A.Selection<int>()
			     .As.Sequence.Array.By.Self.Query()
			     .Skip(300)
			     .Take(100)
			     .WhereBy(x => x > 100)
			     .Out()
			     .Get(source)
			     .Should()
			     .Equal(source.Skip(300)
			                  .Take(100)
			                  .Where(x => x > 100)
			                  .ToArray());
		}

		[Fact]
		void VerifyComprehensive()
		{
			var source = Enumerable.Range(0, 1_000).ToArray();
			Start.A.Selection<int>()
			     .As.Sequence.Array.By.Self.Query()
			     .Skip(300)
			     .Take(200)
			     .WhereBy(x => x > 100)
			     .Skip(50)
			     .Take(100)
			     .Out()
			     .Get(source)
			     .Should()
			     .Equal(source.Skip(300)
			                  .Take(200)
			                  .Where(x => x > 100)
			                  .Skip(50)
			                  .Take(100)
			                  .ToArray());
		}

		public class Benchmarks
		{
			const uint Total = 10_000u;

			uint[] _source;

			[Params(Total)]
			public uint Count
			{
				get => _count;
				set
				{
					_count  = value;
					_source = FixtureInstance.Default.Many<uint>(_count).Get().ToArray();
				}
			}

			uint _count = Total;
			readonly static ISelect<uint[], uint[]> Select = Start.A.Selection<uint>()
			                                                      .As.Sequence.Array.By.Self.Query()
			                                                      .WhereBy(x => x > 1000)
			                                                      .Out();

			[Benchmark]
			public Array Full() => Select.Get(_source);

			[Benchmark]
			public Array FullClassic() => _source.Where(x => x > 1000).ToArray();
		}
	}
}
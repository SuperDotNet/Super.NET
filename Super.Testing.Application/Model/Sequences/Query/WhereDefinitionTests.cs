﻿using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;
using FluentAssertions;
using Super.Compose;
using Super.Model.Selection;
using Super.Testing.Objects;
using Xunit;

namespace Super.Testing.Application.Model.Sequences.Query
{
	public sealed class WhereDefinitionTests
	{
		const uint Total = 1000;
		const int  skip  = 100, take = 100;

		readonly static string[] Source = Data.Default.Get().Take((int)Total).ToArray();

		public class Benchmarks
		{
			const    string              Value    = "ab";
			readonly IEnumerable<string> _classic = Source.Skip(skip).Take(take).Where(x => x.Contains(Value));

			readonly string[] _input;
			readonly ISelect<string[], string[]> _link = Start.A.Selection.Of.Type<string>()
			                                                  .As.Sequence.Array.By.Self.Query()
			                                                  .Skip(skip)
			                                                  .Take(take)
			                                                  .WhereBy(x => x.Contains(Value))
			                                                  .Out();

			public Benchmarks() : this(Source.ToArray()) {}

			public Benchmarks(string[] input) => _input = input;

			[Benchmark(Baseline = true)]
			public string[] Classic() => _classic.ToArray();

			[Benchmark]
			public string[] Subject() => _link.Get(_input);
		}

		[Fact]
		void Verify()
		{
			Start.A.Selection.Of.Type<string>()
			     .As.Sequence.Array.By.Self.Query()
			     .Select(x => x.Length)
			     .Out()
			     .Get(Source)
			     .Should()
			     .Equal(Source.Select(x => x.Length));
		}

		[Fact]
		void VerifyDoubleWhere()
		{
			var numbers = new[] {1, 2, 2, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 7, 7, 7, 7, 7, 7, 7};

			Start.A.Selection.Of.Type<int>()
			     .As.Sequence.Array.By.Self.Query()
			     .WhereBy(x => x > 5)
			     .WhereBy(x => x < 7)
			     .Out()
			     .Get(numbers)
			     .Should()
			     .Equal(numbers.Where(x => x > 5).Where(x => x < 7));
		}

		[Fact]
		void VerifyDoubleWhereSkipTake()
		{
			var numbers = new[] {1, 2, 2, 3, 3, 3, 4, 4, 4, 4, 5, 5, 5, 5, 5, 6, 6, 6, 6, 6, 6, 7, 7, 7, 7, 7, 7, 7};

			Start.A.Selection.Of.Type<int>()
			     .As.Sequence.Array.By.Self.Query()
			     .Skip(10)
			     .Take(12)
			     .WhereBy(x => x > 5)
			     .WhereBy(x => x < 7)
			     .Skip(3)
			     .Take(3)
			     .Out()
			     .Get(numbers)
			     .Should()
			     .Equal(numbers.Skip(10).Take(12).Where(x => x > 5).Where(x => x < 7).Skip(3).Take(3));
		}

		[Fact]
		void VerifySkipTakeWhere()
		{
			Start.A.Selection.Of.Type<string>()
			     .As.Sequence.Array.By.Self.Query()
			     .Skip(skip)
			     .Take(take)
			     .WhereBy(x => x.Contains("ab"))
			     .Out()
			     .Get(Source)
			     .Should()
			     .Equal(Source.Skip(skip).Take(take).Where(x => x.Contains("ab")).ToArray());
		}
	}
}
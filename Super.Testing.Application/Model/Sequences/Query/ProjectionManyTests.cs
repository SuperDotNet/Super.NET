﻿using BenchmarkDotNet.Attributes;
using FluentAssertions;
using Super.Compose;
using Super.Model.Selection;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Super.Testing.Application.Model.Sequences.Query
{
	public sealed class ProjectionManyTests
	{
		readonly static Numbers[] Data = new[]
		{
			new Numbers(1, 2, 3, 4),
			new Numbers(5, 6, 7, 8),
			new Numbers(9, 10)
		};

		[Fact]
		void Verify()
		{
			Start.A.Selection<Numbers>()
			     .As.Sequence.Array.By.Self.Query()
			     .SelectManyBy(x => x.Elements)
			     .Out()
			     .Get(Data)
			     .Should()
			     .Equal(Data.SelectMany(x => x.Elements));
		}

		public sealed class Numbers
		{
			public Numbers(params int[] elements) => Elements = elements;

			public int[] Elements { get; }
		}

		public class Benchmarks
		{
			readonly IEnumerable<int>          _classic;
			readonly ISelect<Numbers[], int[]> _subject;

			public Benchmarks() : this(Data.Hide().SelectMany(x => x.Elements),
			                           Start.A.Selection<Numbers>()
			                                .As.Sequence.Array.By.Self.Query()
			                                .SelectManyBy(x => x.Elements)
			                                .Out()) {}

			public Benchmarks(IEnumerable<int> classic, ISelect<Numbers[], int[]> subject)
			{
				_classic = classic;
				_subject = subject;
			}

			[Benchmark(Baseline = true)]
			public int[] Classic() => _classic.ToArray();

			[Benchmark]
			public int[] Subject() => _subject.Get(Data);
		}
	}
}
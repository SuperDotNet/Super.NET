﻿using BenchmarkDotNet.Attributes;
using FluentAssertions;
using Super.Model.Sequences;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Super.Testing.Application.Model.Collections
{
	public sealed class EnumerateTests
	{
		[Theory]
		[InlineData(1u)]
		[InlineData(2u)]
		[InlineData(3u)]
		[InlineData(4u)]
		[InlineData(5u)]
		[InlineData(8u)]
		[InlineData(16u)]
		[InlineData(32u)]
		[InlineData(64u)]
		[InlineData(128u)]
		[InlineData(256u)]
		[InlineData(512u)]
		[InlineData(1_000u)]
		[InlineData(1_024u)]
		[InlineData(1_025u)]
		/*[InlineData(2_048u)]
		[InlineData(4_096u)]
		[InlineData(8_192u)]
		[InlineData(10_000u)]
		[InlineData(100_000u)]
		[InlineData(1_000_000u)]*/
		void Verify(uint count)
		{
			var numbers = Objects.Numbers.Default.Get(count).Open();
			var open    = numbers.AsEnumerable();
			var store   = Enumerate<int>.Default.Get(open.GetEnumerator());
			new ArrayView<int>(store.Instance, 0, store.Length).ToArray().Should().Equal(numbers);
		}

		public class Benchmarks
		{
			readonly IEnumerate<int>  _enumerate;
			readonly IEnumerable<int> _classic;

			public Benchmarks() : this(Enumerate<int>.Default, Objects.AllNumbers.Default.Get().Take(2048).Hide()) {}

			public Benchmarks(IEnumerate<int> enumerate, IEnumerable<int> classic)
			{
				_enumerate = enumerate;
				_classic   = classic;
			}

			[Benchmark]
			public int[] Classic() => _classic.ToArray();

			[Benchmark]
			public int[] Subject() => _enumerate.Get(_classic.GetEnumerator()).Instance;
		}
	}
}
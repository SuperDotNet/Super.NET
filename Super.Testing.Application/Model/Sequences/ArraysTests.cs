using System;
using System.Linq;
using BenchmarkDotNet.Attributes;
using FluentAssertions;
using Super.Model.Selection;
using Super.Model.Sequences;
using Super.Testing.Objects;
using Xunit;

namespace Super.Testing.Application.Model.Sequences
{
	public sealed class ArraysTests
	{
		public class Benchmarks
		{
			readonly uint[]                       _source;
			readonly ISelect<Store<uint>, uint[]> _sut;

			public Benchmarks() : this(Near.Default) {}

			public Benchmarks(Super.Model.Sequences.Selection selection)
				: this(new Arrays<uint>(selection),
				       FixtureInstance.Default
				                      .Many<uint>(10_000)
				                      .Get()
				                      .ToArray()) {}

			public Benchmarks(ISelect<Store<uint>, uint[]> sut, uint[] source)
			{
				_sut    = sut;
				_source = source;
			}

			[Benchmark]
			public Array Measure() => _sut.Get(_source);
		}

		[Fact]
		void Verify()
		{
			var expected = Enumerable.Range(0, 10_000).ToArray();
			Arrays<int>.Default.Get(expected)
			           .Should()
			           .Equal(expected);
		}

		[Fact]
		void VerifySelection()
		{
			var source   = Enumerable.Range(0, 10_000).ToArray();
			var expected = source.Skip(1000).Take(250).ToArray();
			Arrays<int>.Default.Get(expected)
			           .Should()
			           .Equal(expected);
		}
	}
}
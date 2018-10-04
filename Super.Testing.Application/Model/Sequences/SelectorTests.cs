using BenchmarkDotNet.Attributes;
using FluentAssertions;
using Super.Model.Sequences;
using Super.Testing.Objects;
using System;
using System.Linq;
using Xunit;

namespace Super.Testing.Application.Model.Sequences
{
	public sealed class SelectorTests
	{
		[Fact]
		void Verify()
		{
			var expected = Enumerable.Range(0, 10_000).ToArray();
			Selector<int>.Default.Get(expected)
			                  .Should()
			                  .Equal(expected);
		}

		[Fact]
		void VerifySelection()
		{
			var source   = Enumerable.Range(0, 10_000).ToArray();
			var expected = source.Skip(1000).Take(250).ToArray();
			Selector<int>.Default.Get(expected)
			                  .Should()
			                  .Equal(expected);
		}

		public class Benchmarks
		{
			readonly ISelector<uint> _sut;
			readonly uint[]              _source;

			public Benchmarks() : this(Near.Default) {}

			public Benchmarks(Super.Model.Collections.Selection selection)
				: this(new Selector<uint>(selection),
				       FixtureInstance.Default
				                      .Many<uint>(selection.Start + selection.Length)
				                      .Get()
				                      .ToArray()) {}

			public Benchmarks(ISelector<uint> sut, uint[] source)
			{
				_sut    = sut;
				_source = source;
			}

			[Benchmark]
			public Array Measure() => _sut.Get(_source);
		}
	}
}
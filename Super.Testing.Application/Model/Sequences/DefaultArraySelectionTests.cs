using BenchmarkDotNet.Attributes;
using FluentAssertions;
using Super.Model.Selection.Alterations;
using Super.Model.Sequences;
using Super.Testing.Objects;
using System;
using System.Linq;
using Xunit;

namespace Super.Testing.Application.Model.Sequences
{
	public class DefaultArraySelectionTests
	{
		[Fact]
		void Verify()
		{
			var expected = Enumerable.Range(0, 10_000).ToArray();
			DefaultArraySelection<int>.Default
			                          .Get(expected)
			                          .Should()
			                          .Equal(expected);
		}

		[Fact]
		void VerifySelection()
		{
			var source   = Enumerable.Range(0, 10_000).ToArray();
			var expected = source.Skip(1000).Take(250).ToArray();
			DefaultArraySelection<int>.Default
			                          .Get(expected)
			                          .Should()
			                          .Equal(expected);
		}

		public class Benchmarks
		{
			readonly IAlteration<uint[]> _sut;
			readonly uint[] _source;

			public Benchmarks() : this(Near.Default) {}

			public Benchmarks(Super.Model.Collections.Selection selection)
				: this(new DefaultArraySelection<uint>(selection),
				       FixtureInstance.Default
				                      .Select(new Many<uint>(selection.Start + selection.Length))
				                      .Get()
				                      .ToArray()) {}

			public Benchmarks(IAlteration<uint[]> sut, uint[] source)
			{
				_sut = sut;
				_source = source;
			}

			[Benchmark]
			public Array Measure() => _sut.Get(_source);
		}
	}
}
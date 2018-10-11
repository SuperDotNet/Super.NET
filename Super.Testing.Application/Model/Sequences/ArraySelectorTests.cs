﻿using BenchmarkDotNet.Attributes;
using FluentAssertions;
using Super.Model.Selection.Structure;
using Super.Model.Sequences;
using Super.Testing.Objects;
using System;
using System.Linq;
using Xunit;

namespace Super.Testing.Application.Model.Sequences
{
	public sealed class ArraySelectorTests
	{
		[Fact]
		void Verify()
		{
			var expected = Enumerable.Range(0, 10_000).ToArray();
			ArraySelector<int>.Default.Get(expected)
			                  .Should()
			                  .Equal(expected);
		}

		[Fact]
		void VerifySelection()
		{
			var source   = Enumerable.Range(0, 10_000).ToArray();
			var expected = source.Skip(1000).Take(250).ToArray();
			ArraySelector<int>.Default.Get(expected)
			                  .Should()
			                  .Equal(expected);
		}

		public class Benchmarks
		{
			readonly IStructure<Store<uint>, uint[]> _sut;
			readonly uint[]                  _source;

			public Benchmarks() : this(Near.Default) {}

			public Benchmarks(Super.Model.Collections.Selection selection)
				: this(new ArraySelector<uint>(selection),
				       FixtureInstance.Default
				                      .Many<uint>(10_000)
				                      .Get()
				                      .ToArray()) {}

			public Benchmarks(IStructure<Store<uint>, uint[]> sut, uint[] source)
			{
				_sut    = sut;
				_source = source;
			}

			[Benchmark]
			public Array Measure() => _sut.Get(_source);
		}
	}
}
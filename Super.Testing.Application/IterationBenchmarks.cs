﻿using System;
using System.Collections.Immutable;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Super.Model.Sequences;
using Super.Testing.Objects;

// ReSharper disable NotAccessedVariable
// ReSharper disable RedundantAssignment

namespace Super.Testing.Application
{
	public class IterationBenchmarks
	{
		readonly Array<string>          _array;
		readonly ImmutableArray<string> _immutable;
		readonly string[]               _open;

		public IterationBenchmarks() : this(Data.Default.Get().Take(100).ToArray()) {}

		public IterationBenchmarks(Array<string> array) : this(array, array, array) {}

		public IterationBenchmarks(ImmutableArray<string> immutable, Array<string> array, string[] open)
		{
			_immutable = immutable;
			_array     = array;
			_open      = open;
		}

		[Benchmark]
		public void Array()
		{
			var array  = _array;
			var length = array.Length;
			for (var i = 0u; i < length; i++)
			{
				Call(array[i]);
			}
		}

		[Benchmark]
		public void Immutable()
		{
			var array  = _immutable;
			var length = array.Length;
			for (var i = 0; i < length; i++)
			{
				Call(array[i]);
			}
		}

		[Benchmark(Baseline = true)]
		public void Open()
		{
			var open   = _open;
			var length = open.Length;
			for (var i = 0; i < length; i++)
			{
				Call(open[i]);
			}
		}

		[Benchmark]
		public void Span()
		{
			Span<string> array  = _open;
			var          length = array.Length;
			for (var i = 0; i < length; i++)
			{
				Call(array[i]);
			}
		}

		[Benchmark]
		public void Memory()
		{
			Memory<string> array     = _open;
			var            length    = array.Length;
			var            arraySpan = array.Span;
			for (var i = 0; i < length; i++)
			{
				Call(arraySpan[i]);
			}
		}

		// ReSharper disable once UnusedParameter.Local
		static void Call(string parameter) {}
	}
}
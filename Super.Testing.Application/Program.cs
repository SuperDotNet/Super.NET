using BenchmarkDotNet.Attributes;
using Super.Application.Hosting.BenchmarkDotNet;
using Super.Model.Collections;
using Super.Model.Sources;
using Super.Testing.Objects;
using System;
using System.Linq;

namespace Super.Testing.Application
{
	public class Program
	{
		static void Main()
		{
			Run.Default.Get();
		}
	}

	sealed class Run : Run<Benchmarks>
	{
		public static Run Default { get; } = new Run();

		Run() {}
	}

	public class Benchmarks
	{
		[Benchmark(Baseline = true)]
		public int[] NativeArray() => Testing.Application.NativeArray.Default.Get();

		[Benchmark]
		public ReadOnlyMemory<int> Direct() => Testing.Application.Direct.Default.Get();
	}

	sealed class NativeArray : ISource<int[]>
	{
		public static NativeArray Default { get; } = new NativeArray();

		NativeArray() : this(Select.Default, Data.Default) {}

		readonly Func<string, int> _select;
		readonly string[]          _data;

		public NativeArray(Func<string, int> select, string[] data)
		{
			_select = select;
			_data   = data;
		}

		public int[] Get() => _data.Select(_select).ToArray();
	}

	sealed class Direct : ISource<ReadOnlyMemory<int>>
	{
		public static Direct Default { get; } = new Direct();

		Direct() : this(new ArraySequence<string, int>(Select.Default), View.Default.Get()) {}

		readonly IShape<string, int> _direct;
		readonly ReadOnlyMemory<string>       _view;

		public Direct(IShape<string, int> direct, ReadOnlyMemory<string> view)
		{
			_direct = direct;
			_view   = view;
		}

		public ReadOnlyMemory<int> Get() => _direct.Get(_view);
	}

	sealed class ArraySequence<TFrom, TTo> : IShape<TFrom, TTo> where TTo : unmanaged
	{
		readonly Func<TFrom, TTo> _select;

		public ArraySequence(Func<TFrom, TTo> select) => _select = select;

		public ReadOnlyMemory<TTo> Get(ReadOnlyMemory<TFrom> parameter)
		{
			var length = parameter.Length;
			var span = parameter.Span;
			var store  = new TTo[length];
			for (var i = 0; i < length; i++)
			{
				store[i] = _select(span[i]);
			}

			return store;
		}
	}
}
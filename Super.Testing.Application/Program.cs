using BenchmarkDotNet.Attributes;
using Super.Application.Hosting.BenchmarkDotNet;
using Super.Model.Collections;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Testing.Objects;
using System;
using System.Linq;

namespace Super.Testing.Application
{
	sealed class Program : Run<Benchmarks>
	{
		public static Program Default { get; } = new Program();

		Program() {}

		static void Main()
		{
			/*var x = "asdf".AsReadOnlySpan();
			Console.WriteLine("Hello World!");*/
			Default.Get();
		}
	}



	public class Benchmarks
	{
		[Benchmark(Baseline = true)]
		public int[] NativeArray() => Testing.Application.NativeArray.Default.Get();

		[Benchmark]
		public Array<int> Array() => Testing.Application.Array.Default.Get();

		[Benchmark]
		public int[] Sources() => Testing.Application.Sources.Default.Get();

		[Benchmark]
		public Array<int> Direct() => Testing.Application.Direct.Default.Get();

		[Benchmark]
		public Array<int> Raw() => Testing.Application.Raw.Default.Get();
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

	sealed class Array : ISource<Array<int>>
	{
		public static Array Default { get; } = new Array();

		Array() : this(Select.Default, Data.Default) {}

		readonly IArray<string, int> _array;
		readonly Array<string>       _view;

		public Array(Func<string, int> select, string[] data) : this(new ArraySelect<string, int>(select),
		                                                             new Array<string>(data)) {}

		public Array(IArray<string, int> array, Array<string> view)
		{
			_array = array;
			_view  = view;
		}

		public Array<int> Get() => _array.Get(_view);
	}

	sealed class Sources : ISource<int[]>
	{
		public static Sources Default { get; } = new Sources();

		Sources() : this(new SelectRaw<string, int>(Select.Default), Data.Default) {}

		readonly ISelect<string[], int[]> _sources;
		readonly string[]                 _data;

		public Sources(ISelect<string[], int[]> sources, string[] data)
		{
			_sources = sources;
			_data    = data;
		}

		public int[] Get() => _sources.Get(_data);
	}

	sealed class Direct : ISource<Array<int>>
	{
		public static Direct Default { get; } = new Direct();

		Direct() : this(new ArraySelectDirect<string, int>(Select.Default), View.Default) {}

		readonly IArray<string, int> _direct;
		readonly Array<string>       _view;

		public Direct(IArray<string, int> direct, Array<string> view)
		{
			_direct = direct;
			_view   = view;
		}

		public Array<int> Get() => _direct.Get(_view);
	}

	sealed class Raw : ISource<Array<int>>
	{
		public static Raw Default { get; } = new Raw();

		Raw() : this(new ArraySelectRaw<string, int>(Select.Default), View.Default) {}

		readonly IArray<string, int> _raw;
		readonly Array<string>       _view;

		public Raw(IArray<string, int> raw, Array<string> view)
		{
			_raw  = raw;
			_view = view;
		}

		public Array<int> Get() => _raw.Get(_view);
	}
}
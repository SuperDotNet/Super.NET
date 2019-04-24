using BenchmarkDotNet.Attributes;
using Super.Model.Sequences;
using Super.Testing.Objects;
using System.Collections.Immutable;
// ReSharper disable NotAccessedVariable
// ReSharper disable RedundantAssignment

namespace Super.Testing.Application
{
	public class IterationBenchmarks
	{
		readonly ImmutableArray<string> _immutable;
		readonly Array<string>          _array;
		readonly string[]               _open;

		public IterationBenchmarks() : this(Data.Default.Get()) {}

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
			string current = null;
			var    array   = _array;
			var    length  = array.Length;
			for (var i = 0u; i < length; i++)
			{
				current = array[i];
			}
		}

		[Benchmark]
		public void Immutable()
		{
			string current = null;
			var    length  = _immutable.Length;
			for (var i = 0; i < length; i++)
			{
				current = _immutable[i];
			}
		}

		[Benchmark]
		public void Open()
		{
			string current = null;
			var    array   = _open;
			var    length  = array.Length;
			for (var i = 0; i < length; i++)
			{
				current = array[i];
			}
		}
	}
}
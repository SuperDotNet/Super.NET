using AutoFixture;
using BenchmarkDotNet.Attributes;
using System;
using System.Linq;

namespace Super.Testing.Application
{
	public class CopyArrayBenchmarks<T>
	{
		const uint Total = 10_000u;

		readonly T[] _source = new Fixture().CreateMany<T>((int)Total).ToArray();

		[Params(Total)]
		public uint Count { get; set; }

		[Benchmark]
		public Array For()
		{
			var result = new T[Count];
			for (var i = 0u; i < Count; i++)
			{
				result[i] = _source[i];
			}

			return result;
		}

		[Benchmark]
		public Array Span()
		{
			var result = new T[Count];
			var span = result.AsSpan();
			for (var i = 0; i < Count; i++)
			{
				span[i] = _source[i];
			}

			return result;
		}

		[Benchmark]
		public Array Memory()
		{
			var result = new T[Count];
			var memory = result.AsMemory().Span;
			for (var i = 0; i < Count; i++)
			{
				memory[i] = _source[i];
			}

			return result;
		}

		[Benchmark]
		public Array ToArray() => _source.ToArray();

		[Benchmark]
		public Array Copy()
		{
			var result = new T[Count];
			Array.Copy(_source, 0, result, 0, _source.Length);
			return result;
		}

		/*[Benchmark]
		public Array ConstrainedCopy()
		{
			var result = new string[Count];
			Array.ConstrainedCopy(_source, 0, result, 0, _source.Length);
			return result;
		}*/
	}
}

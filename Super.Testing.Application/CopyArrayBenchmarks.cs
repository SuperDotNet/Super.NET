using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoFixture;
using BenchmarkDotNet.Attributes;

namespace Super.Testing.Application
{
	public class CopyArrayBenchmarks
	{
		const uint Total = 10_000u;

		readonly string[] _source = new Fixture().CreateMany<string>((int)Total).ToArray();

		[Params(Total)]
		public uint Count { get; set; }

		[Benchmark]
		public Array For()
		{
			var result = new string[Count];
			for (var i = 0u; i < Count; i++)
			{
				result[i] = _source[i];
			}

			return result;
		}

		[Benchmark]
		public Array Span()
		{
			var result = new string[Count];
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
			var result = new string[Count];
			var memory = result.AsMemory();
			for (var i = 0; i < Count; i++)
			{
				memory.Span[i] = _source[i];
			}

			return result;
		}

		[Benchmark]
		public Array ConstrainedCopy()
		{
			var result = new string[Count];

			Array.ConstrainedCopy(_source, 0, result, 0, _source.Length);
			return result;
		}
	}
}

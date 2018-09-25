using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Testing.Application
{
	public class IterationBenchmarks
	{
		const uint Total = 1024;

		[Params(Total)]
		/*[Params( /*1u, 2u, 3u, 4u, 5u, 8u, 16u,#1#32u, 64u, 128u, 256u, 512u, 1024u, 1025u, 2048u, 4096u, 8196u,
			10_000u,
			100_000u, 1_000_000u, 10_000_000u, 100_000_000u)]*/
		public uint Count
		{
			get => _count;
			set
			{
				_count  = value;
				_source = Numbers().ToArray();
			}
		}	uint _count = Total;

		uint[] _source;

		/*[Benchmark(Baseline = true)]
		public Array Iterator() => Iterator<uint>.Default.Get(new Iteration<uint>(_source));*/

		/*[Benchmark]
		public Array DynamicIterator() => DynamicIterator<int>.Default.Get(new Iteration<int>(_source));*/

		/*[Benchmark]
		public Array EnumerableToArray() => Numbers().ToArray();*/

		[Benchmark]
		public Array New() => _source.New();

		[Benchmark]
		public Array Copy()
		{
			var length = (int)Count;
			var result = new uint[length];
			Array.Copy(_source, 0, result, 0, length);
			return result;
		}

		/*[Benchmark]
		public Array InstanceCopy()
		{
			var result = new uint[Count];
			_source.CopyTo(result, 0);
			return result;
		}

		[Benchmark]
		public Array ToArray() => _source.ToArray();

		[Benchmark]
		public Array Root()
		{
			using (var enumerable = Numbers().GetEnumerator())
			{
				while (enumerable.MoveNext()) {}
			}

			return null;
		}*/

		IEnumerable<uint> Numbers()
		{
			for (var i = 0u; i < Count; i++)
			{
				yield return i;
			}
		}
	}
}
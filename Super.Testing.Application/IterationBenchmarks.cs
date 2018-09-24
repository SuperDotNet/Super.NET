using BenchmarkDotNet.Attributes;
using Super.Model.Sequences;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Testing.Application
{
	public class IterationBenchmarks
	{
		const uint Total = 10_000u;

		/*[Params(Total)]*/
		[Params( /*1u, 2u, 3u, 4u, 5u, 8u, 16u,*/32u, 64u, 128u, 256u, 512u, 1024u, 1025u, 2048u, 4096u, 8196u,
			10_000u,
			100_000u, 1_000_000u, 10_000_000u, 100_000_000u)]
		public uint Count
		{
			get => _count;
			set
			{
				_count  = value;
				_source = Enumerable.Range(0, (int)Count).ToArray();
			}
		}	uint _count = Total;

		int[] _source;

		[Benchmark(Baseline = true)]
		public Array Iterator() => DynamicIterator<int>.Default.Get(new Iteration<int>(_source));

		/*[Benchmark]
		public Array DynamicIterator() => DynamicIterator<int>.Default.Get(new Iteration<int>(_source));*/

		[Benchmark]
		public Array ToArray() => Numbers().ToArray();

		/*[Benchmark]
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
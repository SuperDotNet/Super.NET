using BenchmarkDotNet.Attributes;
using Super.Model.Selection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Testing.Application
{
	public class EnumerableBenchmarks
	{
		const uint Total = 10_000u;

		readonly ISelect<IEnumerable<uint>, uint[]> _enumerable = In<IEnumerable<uint>>.Start()
		                                                                               .Iterate()
		                                                                               .Skip(500)
		                                                                               .Take(100)
		                                                                               .Reference();

		/*[Params( /*1u, 2u, 3u, 4u,#1# /*5u, 8u, 16u, 32u, 64u, 128u, 256u, 512u, 1024u, 1025u,#1# 2048u, 4096u, 8196u,
			10_000u,
			100_000u, 1_000_000u, 10_000_000u, 100_000_000u)]*/
		/*[Params(1u, 2u, 3u, 4u, 5u)]*/
		[Params(Total)]
		public uint Count { get; set; }

		[Benchmark]
		public Array Classic() => Numbers().Skip(500)
		                                   .Take(100)
		                          //.Hide()
		                          /*.Where(x => x > 1000)
		                          .Skip(8500)
		                          .Take(5)*/
		                          .ToArray();

		/*[Benchmark(Baseline = true)]
		public Array Iteration() => _iteration.Get(_data);*/

		[Benchmark]
		public Array Root()
		{
			using (var enumerable = Numbers() .Skip(500)
			                                  .Take(100)
			                                  .GetEnumerator())
			{
				while (enumerable.MoveNext()) {}
			}

			return null;
		}

		IEnumerable<uint> Numbers()
		{
			for (var i = 0u; i < Count; i++)
			{
				yield return i;
			}
		}

		[Benchmark(Baseline = true)]
		public Array DynamicArray() => _enumerable.Get(Numbers());
	}
}
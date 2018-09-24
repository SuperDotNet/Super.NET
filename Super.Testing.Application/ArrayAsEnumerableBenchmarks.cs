using BenchmarkDotNet.Attributes;
using Super.Model.Selection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Testing.Application
{
	public class ArrayAsEnumerableBenchmarks
	{
		const uint Total = 10_000u;

		//readonly Func<string, int> _selection = Select.Default;

		/*ISelect<Unit, Array<int>> _select;*/
		ISelect<uint[], uint[]> _array;

		uint[] _data;

		/*[Params( /*1u, 2u, 3u, 4u,#1# /*5u, 8u, 16u, 32u, 64u, 128u, 256u, 512u, 1024u, 1025u,#1# 2048u, 4096u, 8196u,
			10_000u,
			100_000u, 1_000_000u, 10_000_000u, 100_000_000u)]*/
		/*[Params(1u, 2u, 3u, 4u, 5u)]*/
		[Params(Total)]
		public uint Count
		{
			get => _count;
			set
			{
				_count = value;
				_data  = Numbers().ToArray();

				_array = In<uint[]>.Start()
				                   .Iterate()
				                   .WhereBy(x => true)
				                   /*.Skip(100)
				                   .Take(5)*/
				                   .Reference();
			}
		}

		uint _count;

		[Benchmark]
		public Array Classic() => _data.Where(x => true)
		                               .Skip(100)
		                               .Take(5)
		                               .ToArray();

		/*[Benchmark]
		public Array IterateFromArrayClassic() => _data.Hide()
		                                          /*.Where(x => x > 1000)
		                                          .Skip(8500)
		                                          .Take(5)#1#
		                                          .ToArray();*/

		/*[Benchmark(Baseline = true)]
		public Array Iteration() => _iteration.Get(_data);*/

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

		[Benchmark(Baseline = true)]
		public Array Current() => _array.Get(_data);
	}
}
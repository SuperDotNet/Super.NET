using AutoFixture;
using BenchmarkDotNet.Attributes;
using Super.Model.Selection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Testing.Application
{
	public class IndexerBenchmarks
	{
		const uint Total = 10_000u;

		//readonly Func<string, int> _selection = Select.Default;

		/*ISelect<Unit, Array<int>> _select;*/
		ISelect<int[], int[]> _array;

		ISelect<IEnumerable<int>, int[]> _enumerable;
		int[]                            _data;

		[Params(/*1u, 2u, 3u, 4u,*/ 5u, 8u, 16u, 32u, 64u, 128u, 256u, 512u, 1024u, 1025u, 2048u, 4096u, 8196u, 10_000u,
			100_000u, 1_000_000u, 10_000_000u, 100_000_000u)]
		/*[Params(1u, 2u, 3u, 4u, 5u)]*/
/*		[Params(Total)]*/
		public uint Count
		{
			get => _count;
			set
			{
				var count = (int)(_count = value);
				_data = Enumerable.Range(0, count)
				                  //.Select(x => string.Empty)
				                  //.Take(count)
				                  .ToArray();
				_array = In<int[]>.Start()
				                  .Iterate()
				                  .WhereBy(x => x > 1000)
				                  .Skip(8500)
				                  .Take(5)
				                  .Reference();

				_enumerable = In<IEnumerable<int>>.Start().Iterate().Reference();

				_numbers = new Fixture().CreateMany<int>((int)Count);
			}
		}

		uint             _count;
		IEnumerable<int> _numbers;

		[Benchmark]
		public Array IterateClassic() => _numbers
			//.Hide()
			/*.Where(x => x > 1000)
			.Skip(8500)
			.Take(5)*/
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
			using (var enumerable = _enumerable.GetEnumerator())
			{
				while (enumerable.MoveNext()) {}
			}

			return null;
		}*/

		[Benchmark(Baseline = true)]
		public Array DynamicArray() => _enumerable.Get(_numbers);
	}
}
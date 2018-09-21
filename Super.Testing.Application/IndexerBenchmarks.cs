using BenchmarkDotNet.Attributes;
using Super.Model.Selection;
using System;
using System.Linq;

namespace Super.Testing.Application
{
	public class IndexerBenchmarks
	{
		const uint Total = 10_000u;

		/*readonly static IEnumerable<string> Strings = Enumerable.Range(0, (int)Total)
		                                                        .Select(x => string.Empty);*/

		//readonly Func<string, int> _selection = Select.Default;

		 /*ISelect<Unit, Array<int>> _select;*/
		ISelect<int[], int[]>     _iteration;
		int[]                     _data;

		/*[Params(1u, 2u, 3u, 4u, 5u, 8u, 16u, 32u, 64u, 128u, 256u, 512u, 1024u, 1025u, 2048u, 4096u, 8196u, 10_000u,
			100_000u, 1_000_000u, 10_000_000u, 100_000_000u)]*/
		/*[Params(1u, 2u, 3u, 4u, 5u)]*/
		[Params(Total)]
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
				/*var @select = _data.ToSource()
				                   .Out()
				                   .AsSelect();*/

				/*_select = @select.Iterate()
				                 .Skip(Count - 5)
				                 .Take(5)
				                 //.Selection(x => 0)
				                 .Release();*/
				/*.Skip(Count - 5)
					.Take(5)*/

				_iteration = In<int[]>.Start()
				                      .Iteration()
				                      .Skip(Count - 5)
				                      .Take(5)
				                      .Reference();
			}
		}

		uint _count;

		[Benchmark]
		public Array IterateClassic() => Enumerable.Range(0, (int)Count) //.Select(_selection)
			/*.Where(x => x > 1000)*/
			.Skip((int)(Count - 5u))
			.Take(5)
			.ToArray();

		
		[Benchmark(Baseline = true)]
		public Array Iteration() => _iteration.Get(_data);
	}
}
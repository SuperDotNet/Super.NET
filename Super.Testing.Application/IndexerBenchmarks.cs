using BenchmarkDotNet.Attributes;
using Super.Model.Selection;
using System;
using System.Linq;

namespace Super.Testing.Application
{
	public class IndexerBenchmarks
	{
		const uint Number = 100_000_000u;

		/*[Params(1u, 2u, 3u, 4u, 5u, 8u, 16u, 32u, 64u, 128u, 256u, 512u, 1024u, 1025u, 2048u, 4096u, 8196u, 10_000u, 100_000u, 1_000_000u, 10_000_000u, 100_000_000u)]*/
		/*[Params(1u, 2u, 3u, 4u, 5u)]*/
		[Params(Number)]
		public uint Count { get; set; }

		/*readonly static string[] Source = Objects.Data.Default.Get();

		readonly static ISelect<Unit, string[]> Strings = Source.ToSource()
		                                                        .Out()
		                                                        .AsSelect();*/

		/*readonly ISelect<Unit, ArraySegment<int>> _select = Strings.Iterate()
		                                                           .Selection(x => x.Length)
			//.Where(x => x > 1000)
			;*/

		readonly ISelect<uint, ArraySegment<int>> _numbers = Objects.Count.Default
		                                                            .Iterate()
		                                                            .Skip(Number - 5)
		                                                            .Take(5);

		[Benchmark(Baseline = true)]
		public Array Segment() => _numbers.Get(Count).ToArray();

		[Benchmark]
		public Array IterateClassic() => Objects.Count.Default.Get(Count) 
		                                        //.Select(x => x.Length)
		                                        //.Where(x => x > 1000)
		                                        .Skip((int)(Count - 5u))
		                                        .Take(5)
		                                        .ToArray();
	}
}
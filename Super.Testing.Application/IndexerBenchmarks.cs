using BenchmarkDotNet.Attributes;
using Super.Model.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;

namespace Super.Testing.Application
{
	public class IndexerBenchmarks
	{
		const uint Total = 10_000_000u;

		readonly static IEnumerable<string> Strings = Enumerable.Range(0, (int)Total)
		                                                        .Select(x => string.Empty);

		ISelect<Unit, ArraySegment<int>> _select;
		uint                             _count;
		string[]                         _data;

		/*[Params(1u, 2u, 3u, 4u, 5u, 8u, 16u, 32u, 64u, 128u, 256u, 512u, 1024u, 1025u, 2048u, 4096u, 8196u, 10_000u, 100_000u, 1_000_000u, 10_000_000u, 100_000_000u)]*/
		/*[Params(1u, 2u, 3u, 4u, 5u)]*/
		[Params(Total)]
		public uint Count
		{
			get => _count;
			set
			{
				var count = (int)(_count = value);
				_data   = Strings.Take(count).ToArray();
				_select = Enumerable.Range(0, count)
				                    .ToArray()
				                    .ToSource()
				                    .Out()
				                    .AsSelect()
				                    .Iterate()
				                    .Skip(Count - 5)
				                    .Take(5);
			}
		}

		[Benchmark(Baseline = true)]
		public Array Iterate() => _select.Get(Unit.Default).ToArray();

		[Benchmark]
		public Array IterateClassic() => Enumerable.Range(0, (int)Count)
		                                           //.Where(x => x > 1000)
		                                           .Skip((int)(Count - 5u))
		                                           .Take(5)
		                                           .ToArray();

/*readonly IArrayBuilder<int> _builder = new ArrayBuilder<int>(Total - 5u, 5);

		[Benchmark(Baseline = true)]
		public Array Iterate() => _builder.Get(new ArrayIterator<int>(_data));

		[Benchmark]
		public Array IterateClassic() => Enumerable.Range(0, int.MaxValue)
		                                           //.Select(x => x.Length)
		                                           //.Where(x => x > 1000)
		                                           .Skip((int)(Count - 5u))
		                                           .Take(5)
		                                           .ToArray();*/
	}
}
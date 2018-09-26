using BenchmarkDotNet.Attributes;
using Super.Model.Collections;
using Super.Model.Selection;
using Super.Model.Sequences;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Testing.Application
{
	public class IterationBenchmarks
	{
		const uint Total = 10_000u;

		readonly ISelect<uint[], Array<uint>> _full;
		readonly ISelect<uint[], uint[]> _selection;
		readonly IEnumerable<uint> _enumerable;

		public IterationBenchmarks() : this(In<uint[]>.Start().Query().Result(),
		                                    In<uint[]>.Start().Query().Skip(5000).Take(300).Get()) {}

		public IterationBenchmarks(ISelect<uint[], Array<uint>> full, ISelect<uint[], uint[]> selection)
		{
			_full      = full;
			_selection = selection;
			_enumerable = Numbers().Skip(5000).Take(300);
		}

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

		[Benchmark]
		public Array<uint> Full() => _full.Get(_source);

		[Benchmark]
		public Array ToArray() => _source.ToArray();


		[Benchmark]
		public Array Selection() => _selection.Get(_source);

		[Benchmark]
		public Array SelectionEnumerable() => _enumerable.ToArray();

		/*[Benchmark]
		public Array New() => _source.New();

		[Benchmark]
		public Array Copy()
		{
			var length = (int)Count;
			var result = new uint[length];
			Array.Copy(_source, 0, result, 0, length);
			return result;
		}*/

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
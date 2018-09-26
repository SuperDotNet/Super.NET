namespace Super.Testing.Application
{
	/*public class IterationBenchmarks
	{
		const uint Total = 10_000u;

		readonly ISelect<uint[], Array<uint>> _full;
		readonly ISelect<uint[], uint[]>      _near, _far;

		IEnumerable<uint> _nearSource, _farSource;

		public IterationBenchmarks() : this(In<uint[]>.Start().Query().Result(),
		                                    In<uint[]>.Start().Query().Skip(300).Take(100).Get(),
		                                    In<uint[]>.Start().Query().Skip(5000).Take(300).Get()) {}

		public IterationBenchmarks(ISelect<uint[], Array<uint>> full,
		                           ISelect<uint[], uint[]> near, ISelect<uint[], uint[]> far)
		{
			_full = full;
			_near = near;
			_far  = far;
		}

		[Params(Total)]
		/*[Params( /*1u, 2u, 3u, 4u, 5u, 8u, 16u,#2#32u, 64u, 128u, 256u, 512u, 1024u, 1025u, 2048u, 4096u, 8196u,
			10_000u,
			100_000u, 1_000_000u, 10_000_000u, 100_000_000u)]#1#
		public uint Count
		{
			get => _count;
			set
			{
				_count      = value;
				_source     = Numbers().ToArray();
				_nearSource = Numbers().Skip(300).Take(100);
				_farSource  = Numbers().Skip(5000).Take(300);
			}
		}

		uint _count = Total;

		uint[] _source;

		[Benchmark]
		public Array<uint> Full() => _full.Get(_source);

		[Benchmark]
		public Array Near() => _near.Get(_source);

		[Benchmark]
		public Array Far() => _far.Get(_source);

		[Benchmark]
		public Array ToArray() => _source.ToArray();

		[Benchmark]
		public Array NearEnumerable() => _nearSource.ToArray();

		[Benchmark]
		public Array FarEnumerable() => _farSource.ToArray();

		/*[Benchmark]
		public Array New() => _source.New();

		[Benchmark]
		public Array Copy()
		{
			var length = (int)Count;
			var result = new uint[length];
			Array.Copy(_source, 0, result, 0, length);
			return result;
		}#1#

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
		}#1#

		IEnumerable<uint> Numbers()
		{
			for (var i = 0u; i < Count; i++)
			{
				yield return i;
			}
		}
	}*/
}
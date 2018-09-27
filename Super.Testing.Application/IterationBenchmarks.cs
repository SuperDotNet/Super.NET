using BenchmarkDotNet.Attributes;
using Super.Reflection;
using Super.Testing.Objects;
using System;
using System.Linq;

namespace Super.Testing.Application
{
	public class IterationBenchmarks
	{
		readonly Func<uint, Enumerations<uint>> _classics;
		readonly Sequencing<uint>               _subject;

		Enumerations<uint> _classic;

		uint[] _source;

		public IterationBenchmarks() : this(Numbers.Default
		                                           .Select(x => x.Select(y => (uint)y))
		                                           .Select(I<ArrayEnumerations<uint>>.Default.From)
		                                           .Get,
		                                    Sequencing<uint>.Default) {}

		public IterationBenchmarks(Func<uint, Enumerations<uint>> classics, Sequencing<uint> subject)
		{
			_classics = classics;
			_subject  = subject;
		}

		[Params(1u, 2u, 3u, 4u, 5u, 8u, 16u, 32u, 64u, 128u, 256u, 512u, 1024u, 1025u, 2048u, 4096u, 8196u,
			10_000u, 100_000u, 1_000_000u, 10_000_000u, 100_000_000u)]
		public uint Count
		{
			get => _count;
			set
			{
				_classic = _classics(_count = value);
				_source  = _classic.Full.ToArray();
			}
		}

		uint _count;

		[Benchmark]
		public Array Full() => _subject.Full.Get(_source);

		[Benchmark]
		public Array FullClassic() => _classic.Full.ToArray();
	}
}
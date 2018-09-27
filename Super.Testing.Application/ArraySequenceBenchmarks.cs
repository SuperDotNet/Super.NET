using BenchmarkDotNet.Attributes;
using Super.Reflection;
using Super.Testing.Objects;
using System;
using System.Linq;

namespace Super.Testing.Application
{
	public class ArraySequenceBenchmarks : SequenceBenchmarks<uint>
	{
		public ArraySequenceBenchmarks() : base(I<ArrayEnumerations<uint>>.Default.From) {}
	}

	public class SequenceBenchmarks<T>
	{
		const uint Total = 10_000u;

		readonly Func<uint, Enumerations<T>> _classics;
		readonly Sequencing<T> _subject;

		Enumerations<T> _classic;

		T[] _source;

		public SequenceBenchmarks(Func<uint, Enumerations<T>> classics) : this(classics, Sequencing<T>.Default) {}

		public SequenceBenchmarks(Func<uint, Enumerations<T>> classics, Sequencing<T> subject)
		{
			_classics = classics;
			_subject = subject;
		}

		[Params(Total)]
		public uint Count
		{
			get => _count;
			set
			{
				_count   = value;
				_classic = _classics(_count);
				_source  = _classic.Full.ToArray();
			}
		}

		uint _count = Total;

		[Benchmark]
		public Array Full() => _subject.Full.Get(_source);

		[Benchmark]
		public Array FullClassic() => _classic.Full.ToArray();

		[Benchmark]
		public Array Near() => _subject.Near.Get(_source);

		[Benchmark]
		public Array NearClassic() => _classic.Near.ToArray();

		[Benchmark]
		public Array Far() => _subject.Far.Get(_source);

		[Benchmark]
		public Array FarClassic() => _classic.Far.ToArray();
	}
}
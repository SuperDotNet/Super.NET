using AutoFixture;
using BenchmarkDotNet.Attributes;
using Super.Model.Collections;
using Super.Model.Selection;
using Super.Model.Sequences;
using Super.Model.Sources;
using Super.Reflection;
using Super.Runtime.Activation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Testing.Application
{
	public sealed class Sequencing<T>
	{
		public static Sequencing<T> Default { get; } = new Sequencing<T>();

		Sequencing() : this(In<T[]>.Start().Query(), Testing.Application.Near.Default,
		                    Testing.Application.Far.Default) {}

		public Sequencing(Super.Model.Sequences.Query<T[], T> query, Selection near, Selection far)
			: this(query.Result(), query.Select(near).Get(), query.Select(far).Get()) {}

		public Sequencing(ISelect<T[], Array<T>> full, ISelect<T[], T[]> near, ISelect<T[], T[]> far)
		{
			Full = full;
			Near = near;
			Far  = far;
		}

		public ISelect<T[], Array<T>> Full { get; }
		public ISelect<T[], T[]> Near { get; }
		public ISelect<T[], T[]> Far { get; }
	}

	sealed class Near : Source<Selection>
	{
		public static Near Default { get; } = new Near();

		Near() : base(new Selection(300, 100)) {}
	}

	sealed class Far : Source<Selection>
	{
		public static Far Default { get; } = new Far();

		Far() : base(new Selection(5000, 300)) {}
	}

	sealed class FixtureInstance : Source<IFixture>
	{
		public static FixtureInstance Default { get; } = new FixtureInstance();

		FixtureInstance() : base(new Fixture()) {}
	}

	sealed class ArrayEnumerations<T> : Enumerations<T>, IActivateMarker<uint>
	{
		public ArrayEnumerations(uint count)
			: base(FixtureInstance.Default.Get().CreateMany<T>((int)count).ToArray()) {}
	}

	// ReSharper disable all PossibleMultipleEnumeration
	public class Enumerations<T>
	{
		public Enumerations(uint count) : this(FixtureInstance.Default.Get().CreateMany<T>((int)count)) {}

		public Enumerations(IEnumerable<T> source)
			: this(source, Testing.Application.Near.Default, Testing.Application.Far.Default) {}

		public Enumerations(IEnumerable<T> source, Selection near, Selection far)
			: this(source,
			       source.Skip((int)near.Start).Take((int)near.Length.GetValueOrDefault()),
			       source.Skip((int)far.Start).Take((int)far.Length.GetValueOrDefault())) {}

		public Enumerations(IEnumerable<T> full, IEnumerable<T> near, IEnumerable<T> far)
		{
			Full = full;
			Near = near;
			Far  = far;
		}

		public IEnumerable<T> Full { get; }
		public IEnumerable<T> Near { get; }
		public IEnumerable<T> Far { get; }
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
		public Array<T> Full() => _subject.Full.Get(_source);

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

	public class ArraySequenceBenchmarks : SequenceBenchmarks<uint>
	{
		public ArraySequenceBenchmarks() : base(I<ArrayEnumerations<uint>>.Default.From) {}
	}
}
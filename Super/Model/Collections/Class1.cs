using Super.Model.Selection;
using Super.Model.Selection.Stores;
using Super.Model.Sources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Model.Collections
{
	public interface IArray<T> : ISource<Array<T>> {}

	public class ArrayInstance<T> : Source<Array<T>>, IArray<T>
	{
		public ArrayInstance(IEnumerable<T> enumerable) : this(enumerable.ToArray()) {}

		public ArrayInstance(params T[] instance) : base(instance) {}
	}

	public class DelegatedArray<T> : DelegatedSource<Array<T>>, IArray<T>
	{
		public DelegatedArray(Func<Array<T>> source) : base(source) {}
	}

	public class DecoratedArray<T> : DecoratedSource<Array<T>>, IArray<T>
	{
		public DecoratedArray(ISource<Array<T>> source) : base(source) {}
	}

	public interface IArray<in TFrom, TItem> : ISelect<TFrom, Array<TItem>> {}

	public class ArrayStore<TFrom, TTo> : Store<TFrom, Array<TTo>>, IArray<TFrom, TTo>
	{
		public ArrayStore(Func<TFrom, Array<TTo>> source) : base(source) {}
	}

	public class Array<TFrom, TItem> : IArray<TFrom, TItem>
	{
		readonly Func<TFrom, IEnumerable<TItem>> _selector;

		public Array(Func<TFrom, IEnumerable<TItem>> selector) => _selector = selector;

		public Array<TItem> Get(TFrom parameter) => _selector(parameter).ToArray();
	}

	public interface ISequence<out T> : ISource<IEnumerable<T>> {}

	public class Sequence<T> : Source<IEnumerable<T>>, ISequence<T>
	{
		public Sequence(IEnumerable<T> instance) : base(instance) {}
	}

	public class DecoratedSequence<T> : DecoratedSource<IEnumerable<T>>, ISequence<T>
	{
		public DecoratedSequence(ISource<IEnumerable<T>> instance) : base(instance) {}
	}

	public interface ISequence<in TFrom, out TItem> : ISelect<TFrom, IEnumerable<TItem>> {}

	sealed class Access<T> : Select<IEnumerable<T>, Array<T>>
	{
		public static Access<T> Default { get; } = new Access<T>();

		Access() : base(x => x.ToArray()) {}
	}

	sealed class Result<T> : ISelect<IEnumerable<T>, Array<T>>
	{
		public static Result<T> Default { get; } = new Result<T>();

		Result() {}

		public Array<T> Get(IEnumerable<T> parameter) => new Array<T>(parameter.ToArray());
	}

	sealed class ClassicTake<T> : ISelect<uint, IEnumerable<T>>
	{
		readonly Func<IEnumerable<T>> _source;

		public ClassicTake(Func<IEnumerable<T>> source) => _source = source;

		public IEnumerable<T> Get(uint parameter) => _source().Take((int)parameter);
	}

	/*class Decorate<T> : ISequencer<T>
	{
		readonly ISequencer<T> _sequencer;

		public Decorate(ISequencer<T> sequencer) => _sequencer = sequencer;

		public ReadOnlyMemory<T> Get(IEnumerable<T> parameter) => _sequencer.Get(parameter);
	}*/

	/*sealed class Immutable<T>  : DecoratedSelect<ReadOnlyMemory<T>, ImmutableArray<T>>
	{
		public static Immutable<T> Default { get; } = new Immutable<T>();

		Immutable() : base(Enumerate<T>.Default.Select(x => x.ToImmutableArray())) {}
	}

	sealed class Enumerate<T> : Select<ReadOnlyMemory<T>, IEnumerable<T>>
	{
		public static Enumerate<T> Default { get; } = new Enumerate<T>();

		Enumerate() : base(x => x.AsEnumerable()) {}
	}*/
}
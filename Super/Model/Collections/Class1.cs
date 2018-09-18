using Super.Model.Selection;
using Super.Model.Selection.Stores;
using Super.Model.Sources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Model.Collections
{
	public interface IArrays<T> : ISource<ReadOnlyMemory<T>> {}

	public class Arrays<T> : Source<ReadOnlyMemory<T>>, IArrays<T>
	{
		public Arrays(IEnumerable<T> enumerable) : this(enumerable.ToArray()) {}

		public Arrays(params T[] instance) : base(instance) {}
	}

	public class DelegatedArrays<T> : DelegatedSource<ReadOnlyMemory<T>>, IArrays<T>
	{
		public DelegatedArrays(Func<ReadOnlyMemory<T>> source) : base(source) {}
	}

	public class DecoratedArrays<T> : DecoratedSource<ReadOnlyMemory<T>>, IArrays<T>
	{
		public DecoratedArrays(ISource<ReadOnlyMemory<T>> source) : base(source) {}
	}

	public interface IArray<in TFrom, TItem> : ISelect<TFrom, ReadOnlyMemory<TItem>> {}

	public class ArrayStore<TFrom, TTo> : Store<TFrom, ReadOnlyMemory<TTo>>, IArray<TFrom, TTo>
	{
		public ArrayStore(Func<TFrom, ReadOnlyMemory<TTo>> source) : base(source) {}
	}

	public class Array<TFrom, TItem> : IArray<TFrom, TItem>
	{
		readonly Func<TFrom, IEnumerable<TItem>> _selector;

		public Array(Func<TFrom, IEnumerable<TItem>> selector) => _selector = selector;

		public ReadOnlyMemory<TItem> Get(TFrom parameter) => _selector(parameter).ToArray();
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

	sealed class Access<T> : Select<IEnumerable<T>, ReadOnlyMemory<T>>
	{
		public static Access<T> Default { get; } = new Access<T>();

		Access() : base(x => x.ToArray()) {}
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
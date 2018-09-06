using Super.Model.Selection;
using Super.Model.Selection.Stores;
using Super.Model.Sources;
using Super.Reflection.Types;
using Super.Runtime.Environment;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Model.Collections
{
	public interface ISequence<in TFrom, TItem> : ISelect<TFrom, ReadOnlyMemory<TItem>> {}

	public class SequenceStore<TFrom, TTo> : Select<TFrom, ReadOnlyMemory<TTo>>, ISequence<TFrom, TTo>
	{
		public SequenceStore(IStream<TFrom, TTo> stream) : this(stream.ToSequence().Get) {}

		public SequenceStore(Func<TFrom, ReadOnlyMemory<TTo>> source)
			: base(new Store<TFrom, ReadOnlyMemory<TTo>>(source).Get) {}
	}

	public class Sequence<TFrom, TItem> : ISequence<TFrom, TItem>
	{
		readonly Func<TFrom, IEnumerable<TItem>> _selector;

		public Sequence(Func<TFrom, IEnumerable<TItem>> selector) => _selector = selector;

		public ReadOnlyMemory<TItem> Get(TFrom parameter) => _selector(parameter).ToArray();
	}

	public interface IStream<in TFrom, out TItem> : ISelect<TFrom, IEnumerable<TItem>> {}

	public interface ISequential<in TFrom, TItem> : ISelect<TFrom, ISequence<TItem>> {}

	public interface ISequence<T> : ISource<ReadOnlyMemory<T>> {}

	public interface IShape<TFrom, TTo> : ISelect<ReadOnlyMemory<TFrom>, ReadOnlyMemory<TTo>> {}

	static class Implementations
	{
		public static ISelect<Type, uint> Size { get; } = DefaultComponent<ISize>.Default.Get().ToStore();
	}

	sealed class Size<T> : FixedDeferredSingleton<Type, uint>
	{
		public static Size<T> Default { get; } = new Size<T>();

		Size() : base(Implementations.Size, Type<T>.Instance) {}
	}

	public interface ISize : ISelect<Type, uint> {}
}
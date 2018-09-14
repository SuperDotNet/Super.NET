using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Super.Model.Selection;

namespace Super.Model.Collections
{
	public interface IIndexer<T> : ISelect<IIndex<T>, ReadOnlyMemory<T>> {}

	sealed class Indexer<T> : IIndexer<T>
	{
		public static Indexer<T> Default { get; } = new Indexer<T>();

		Indexer() : this(1024) {}

		readonly uint _size;

		public Indexer(in uint size) => _size = size;

		public ReadOnlyMemory<T> Get(IIndex<T> parameter)
		{
			var count = 0u;
			var page  = new Page<T>(in _size);
			var size  = page.Size;
			while (count < size && parameter.Next(in count, out var element))
			{
				page[count++] = element;
			}

			var first = page.Get(in count);
			return count < size ? first : new Indexing<T>(parameter).Compile(first);
		}
	}

	public interface IIterator<T> : ISelect<IEnumerable, ReadOnlyMemory<T>> {}

	public interface IIndexes<T> : ISelect<IEnumerable, IIndex<T>> {}

	sealed class Indexes<T> : IIndexes<T>
	{
		public static Indexes<T> Default { get; } = new Indexes<T>();

		Indexes() {}

		public IIndex<T> Get(IEnumerable parameter)
		{
			switch (parameter)
			{
				case T[] array:
					return new ArrayIndex<T>(array);
				case IEnumerable<T> enumerable:
					return new Index<T>(enumerable.GetEnumerator());
			}
			throw new InvalidOperationException($"Unsupported index type: {parameter.GetType().FullName}");
		}
	}

	sealed class Iterator<T> : IIterator<T>
	{
		readonly IIndexes<T> _indexes;
		readonly IIndexer<T> _indexer;

		public static Iterator<T> Default { get; } = new Iterator<T>();

		Iterator() : this(Indexes<T>.Default, Indexer<T>.Default) {}

		public Iterator(IIndexes<T> indexes, IIndexer<T> indexer)
		{
			_indexes = indexes;
			_indexer = indexer;
		}

		public ReadOnlyMemory<T> Get(IEnumerable parameter) => _indexer.Get(_indexes.Get(parameter));
	}

	public interface IIndex<T> : IIndex<uint, T> {}

	sealed class Index<T> : IIndex<T>
	{
		readonly IEnumerator<T> _enumerator;
		readonly T _default;

		public Index(IEnumerator<T> enumerator) : this(enumerator, Sources.Default<T>.Instance) {}

		public Index(IEnumerator<T> enumerator, T @default)
		{
			_enumerator = enumerator;
			_default = @default;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Next(in uint index, out T element)
		{
			var result = _enumerator.MoveNext();
			element = result ? _enumerator.Current : _default;
			return result;
		}
	}

	sealed class ArrayIndex<T> : IIndex<T>
	{
		readonly T[]  _source;
		readonly uint _length;
		readonly T _default;

		public ArrayIndex(T[] source) : this(source, (uint)source.Length, Sources.Default<T>.Instance) {}

		public ArrayIndex(T[] source, in uint length, in T @default)
		{
			_source = source;
			_length = length;
			_default = @default;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Next(in uint index, out T element)
		{
			var result = index < _length;
			element = result ? _source[index] : _default;
			return result;
		}
	}

	public interface IIndex<TIn, TOut>
	{
		bool Next(in TIn index, out TOut element);
	}
}

using Super.Model.Selection;
using Super.Runtime;
using System;
using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Super.Model.Collections
{
	public interface IEnumerate<T> : ISelect<IEnumerator<T>, View<T>> {}

	sealed class Enumerate<T> : IEnumerate<T>
	{
		public static Enumerate<T> Default { get; } = new Enumerate<T>();

		Enumerate() : this(ArrayPool<T>.Shared, ArrayPool<View<T>>.Shared) {}

		readonly ArrayPool<T> _pool;
		readonly ArrayPool<View<T>> _views;
		readonly uint _size;

		public Enumerate(ArrayPool<T> pool, ArrayPool<View<T>> views, in uint size = 1024)
		{
			_pool = pool;
			_views = views;
			_size = size;
		}

		public View<T> Get(IEnumerator<T> parameter)
		{
			if (!parameter.MoveNext())
			{
				return new View<T>(Empty<T>.Array);
			}

			var one = parameter.Current;

			if (!parameter.MoveNext())
			{
				return new View<T>(one);
			}

			var two = parameter.Current;
			if (!parameter.MoveNext())
			{
				return new View<T>(one, two);
			}

			var three = parameter.Current;
			if (!parameter.MoveNext())
			{
				return new View<T>(one, two, three);
			}

			var four = parameter.Current;
			if (!parameter.MoveNext())
			{
				return new View<T>(one, two, three, four);
			}

			var five = parameter.Current;

			var items = _pool.Rent((int)_size);
			items[0] = one;
			items[1] = two;
			items[2] = three;
			items[3] = four;
			items[4] = five;
			var view = new View<T>(items, _pool);
			var size = items.Length;
			var count = 5u;
			while (count < size && parameter.MoveNext())
			{
				items[count++] = parameter.Current;
			}

			var seed = view.Resize(in count);
			return count < size ? seed : Views(seed).Compile(parameter);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		Views<T> Views(View<T> first)
		{
			var store = _views.Rent(32);
			store[0] = first;
			return new Views<T>(new View<View<T>>(store, _views), _pool);
		}
	}

	/*public interface IIndexes<T> : ISelect<IEnumerable, IIndex<T>> {}

	sealed class Indexes<T> : IIndexes<T>
	{
		public static Indexes<T> Default { get; } = new Indexes<T>();

		Indexes() {}

		public IIndex<T> Get(IEnumerable parameter)
		{
			switch (parameter)
			{
				case ICollection<T>:
					return null;
				case T[] array:
					return new ArrayIndex<T>(array);
				case IEnumerable<T> enumerable:
					return new Index<T>(enumerable.GetEnumerator());
			}
			throw new InvalidOperationException($"Unsupported index type: {parameter.GetType().FullName}");
		}
	}*/

	/*public interface IIterator<T> : ISelect<IEnumerable, ReadOnlyMemory<T>> {}

	sealed class Iterator<T> : IIterator<T>
	{
		readonly IIndexes<T> _indexes;
		readonly IIndexer<T> _indexer;

		public static Iterator<T> Default { get; } = new Iterator<T>();

		Iterator() : this(Indexer<T>.Default, Indexes<T>.Default) {}

		public Iterator(IIndexer<T> indexer, IIndexes<T> indexes)
		{
			_indexes = indexes;
			_indexer = indexer;
		}

		public ReadOnlyMemory<T> Get(IEnumerable parameter) => _indexer.Get(_indexes.Get(parameter));
	}*/

	/*public interface IIndex<T> : IIndex<uint, T> {}

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
	}*/

	public interface ILoad<T> : ISelect<IEnumerable, View<T>> {}

	sealed class Load<T> : ILoad<T>
	{
		public static Load<T> Default { get; } = new Load<T>();

		Load() : this(Enumerate<T>.Default, ArrayPool<T>.Shared) {}

		readonly IEnumerate<T> _enumerate;
		readonly ArrayPool<T> _pool;

		public Load(IEnumerate<T> enumerate, ArrayPool<T> pool)
		{
			_enumerate = enumerate;
			_pool = pool;
		}

		public View<T> Get(IEnumerable parameter)
		{
			switch (parameter)
			{
				case T[] array:
					return new View<T>(array, array.AsMemory());
				case ICollection<T> collection:
					var rental = _pool.Rent(collection.Count);
					collection.CopyTo(rental, 0);
					return new View<T>(rental, rental.AsMemory(0, collection.Count), _pool);
				case IEnumerable<T> enumerable:
					return _enumerate.Get(enumerable.GetEnumerator());
			}
			throw new InvalidOperationException($"Unsupported view type: {parameter.GetType().FullName}");
		}
	}

	public readonly struct View<T>
	{
		readonly T[] _store;
		readonly ArrayPool<T> _pool;
		readonly Memory<T> _view;

		public View(params T[] items) : this(items, null) {}

		public View(T[] store, ArrayPool<T> pool = null) : this(store, store, pool) {}

		public View(T[] store, Memory<T> view, ArrayPool<T> pool = null)
		{
			_store = store;
			_pool = pool;
			_view = view;

			Total = (uint)_store.Length;
			Used = (uint)view.Length;
		}

		public uint Used { get; }

		public uint Total { get; }

		public ref T this[in uint index] => ref _store[index];

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Copy(in View<T> source, in uint offset)
		{
			var sourceView = source._view;
			var memory = _view;
			sourceView.CopyTo(memory.Slice((int)offset, sourceView.Length));
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Release()
		{
			_pool?.Return(_store);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public View<T> Resize(in uint length) => new View<T>(_store, (_view).Slice(0, (int)length), _pool);

		/*[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ImmutableArray<T> Get()
		{
			var result = (_view).ToArray().ToImmutableArray();
			return result;
		}*/

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T[] Emit()
		{
			var result = (_view).ToArray();
			Release();
			return result;
		}
	}

	/*sealed class ArrayIndex<T> : IIndex<T>
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
	}*/

	public interface IIndex<TIn, TOut>
	{
		bool Next(in TIn index, out TOut element);
	}
}

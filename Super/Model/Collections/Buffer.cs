using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace Super.Model.Collections
{
	public readonly ref struct Sequenced<T>
	{
		readonly T[]     _store;
		readonly Span<T> _span;

		public Sequenced(int size) : this(ArrayPool<T>.Shared.Rent(size)) {}

		public Sequenced(T[] store) : this(store, store, store.Length) {}

		public Sequenced(T[] store, Span<T> span, int size)
		{
			Size = size;
			_store = store;
			_span  = span;
		}

		public int Size { get; }

		public ref T this[uint index] => ref _span[(int)index];

		/*[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool Add(in T element, ref uint index)
		{
			_store[index] = element;
			return ++index < _size;
		}*/

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T[] Get(in uint index)
		{
			if (index != Size)
			{
				var span   = _span;
				var result = span.Slice(0, (int)index).ToArray();
				ArrayPool<T>.Shared.Return(_store);
				return result;
			}

			return _store;
		}
	}

	/*public static class Extensions
	{
		/*public static TTo[] Select<TFrom, TTo>(this Sequenced<TTo> @this, TFrom[] source, Func<TFrom, TTo> select, ref uint index)
			=> Select(@this, source, source.Length, @select, ref index);#1#

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static TTo[] Select<TFrom, TTo>(this Sequenced<TTo> @this, TFrom[] source, Func<TFrom, TTo> select, ref uint index)
		{
			var length = source.Length;
			for (; index < length; index++)
			{
				@this.Add(select(source[index]), in index);
			}

			return @this.Get(in index);
		}
	}*/

	/*readonly ref struct Materializer<T>
	{
		/*readonly T[] _source;
		readonly uint _target;

		public Materializer(T[] source, in uint target)
		{
			_source = source;
			_target = target;
		}#1#

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public TTo[] Select<TTo>(

			Func<T, TTo> select, in int size, ref uint index)
		{

		}
	}*/

	ref struct Buffer<T>
	{
		readonly T[][] _store;

		readonly int _pageSize;

		int          _pageIndex, _itemIndex, _itemCount;

		public Buffer(uint pages, int pageSize = 1024) : this(new T[1][]/*ArrayPool<T[]>.Shared.Rent((int)pages)*/, pageSize) {}

		Buffer(T[][] store, int pageSize) : this()
		{
			_store = store;
			_pageSize = pageSize;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Append(in T element)
		{
			var current = _store[_pageIndex] ?? (_store[_pageIndex] = new T[10]);

			current[_itemIndex] = element;

			_itemCount++;
			_itemIndex++;

			if (_itemIndex >= _store[_pageIndex].Length)
			{
				_pageIndex++;
				_itemIndex = 0;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		T[] Rent() => ArrayPool<T>.Shared.Rent(/*_pageIndex == 0 ? 16 : Math.Max(_pageSize, _itemCount * 2)*/10);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T[] Flush()
		{
			var result = new T[_itemCount];
			for (int i = 0, index = 0; _itemCount > 0; i++)
			{
				var size = _store[i].Length;
				var amount = Math.Min(_itemCount, size);
				Array.Copy(_store[i], 0, result, index, amount);
				_itemCount -= amount;
				index += size;
			}

			//ArrayPool<T>.Shared.Return(_store);
			return result;
		}
	}
}
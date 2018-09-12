using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace Super.Model.Collections
{
	/*readonly ref struct BufferReference<T> // : IDisposable
	{
		readonly ArrayPool<T> _pool;
		readonly T[]          _source;
		readonly Span<T>      _page;
		readonly int          _index;

		public BufferReference(int pageSize, int index = 0) : this(ArrayPool<T>.Shared, pageSize, index) {}

		public BufferReference(ArrayPool<T> pool, int pageSize, int index = 0) : this(pool, pool.Rent(pageSize), index) {}

		public BufferReference(ArrayPool<T> pool, T[] source, int index = 0) : this(pool, source, source, index) {}

		public BufferReference(ArrayPool<T> pool, T[] source, Span<T> page, int index = 0)
		{
			_pool   = pool;
			_source = source;
			_page   = page;
			_index  = index;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public BufferReference<T> Add(T item)
		{
			_page[_index] = item;
			return new BufferReference<T>(_pool, _source, _page, _index + 1);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T[] Get()
		{
			var result = new T[_index];
			Array.Copy(_source, 0, result, 0, _index);
			/*var array = _page;
			array.Slice(0, _index).CopyTo(result);#1#
			_pool.Return(_source);
			return result;
		}

		/*[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Dispose()
		{
			_pool.Return(_page);
		}#1#
	}*/

	/*readonly struct BufferingCopy<T> : IDisposable
	{
		readonly ArrayPool<T> _pool;
		readonly T[]          _page;
		readonly int          _index;

		public BufferingCopy(int pageSize, int index = 0) : this(ArrayPool<T>.Shared, pageSize, index) {}

		public BufferingCopy(ArrayPool<T> pool, int pageSize, int index = 0) : this(pool, pool.Rent(pageSize), index) {}

		public BufferingCopy(ArrayPool<T> pool, T[] page, int index = 0)
		{
			_pool  = pool;
			_page  = page;
			_index = index;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public BufferingCopy<T> Add(T item)
		{
			_page[_index] = item;
			return new BufferingCopy<T>(_pool, _page, _index + 1);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T[] Get() => _page.AsSpan(0, _index).ToArray();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Dispose()
		{
			_pool.Return(_page);
		}
	}*/

	/*ref struct PageCursor
	{
		/*public PageCursor(uint pageSize = 1024) : this() => PageSize = pageSize;

		public uint PageIndex { get; set; }

		public uint ItemIndex { get; set; }

		public uint ItemCount { get; set; }


		public uint Pages { get; set; }

		public uint PageSize { get; set; }#1#

		public PageCursor(uint pages, int pageSize = 1024) : this()
		{
			Pages     = pages;
			PageSize  = pageSize;
		}


	}*/

	ref struct Buffer<T>
	{
		readonly T[][] _store;

		readonly int _pageSize;

		int          _pageIndex, _itemIndex, _itemCount;

		public Buffer(uint pages, int pageSize = 1024) : this(new T[pages][], pageSize) {}

		Buffer(T[][] store, int pageSize) : this()
		{
			_store = store;
			_pageSize = pageSize;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Append(in T element)
		{
			var current = _store[_pageIndex] ?? (_store[_pageIndex] = ArrayPool<T>.Shared.Rent(_pageSize));

			current[_itemIndex] = element;

			_itemCount++;
			_itemIndex++;

			if (_itemIndex >= _pageSize)
			{
				_pageIndex++;
				_itemIndex = 0;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T[] Flush()
		{
			var result = new T[_itemCount];
			for (int i = 0, index = 0; _itemCount > 0; ArrayPool<T>.Shared.Return(_store[i++]), index += _pageSize)
			{
				var amount = Math.Min(_itemCount, _pageSize);
				Array.Copy(_store[i], 0, result, index, amount);
				_itemCount -= amount;
			}
			return result;
		}
	}
}
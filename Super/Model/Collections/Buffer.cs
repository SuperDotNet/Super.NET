using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace Super.Model.Collections
{
	public interface IInstance<TIn, out TOut>
	{
		TOut Get(in TIn parameter);
	}

	public readonly ref struct Page<T>
	{
		readonly T[] _store;

		public Page(in uint size) : this(ArrayPool<T>.Shared.Rent((int)size)) {}

		public Page(T[] store) => _store = store;

		public uint Size => (uint)_store.Length;

		public ref T this[in uint index] => ref _store[index];

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T[] Get(in uint index)
		{
			if (index != _store.Length)
			{
				var result = View(in index).ToArray();
				Clear();
				return result;
			}

			return _store;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Clear()
		{
			ArrayPool<T>.Shared.Return(_store);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ReadOnlyMemory<T> View(in uint index) => _store.AsMemory().Slice(0, (int)index);
	}
}
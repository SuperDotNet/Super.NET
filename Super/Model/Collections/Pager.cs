using System;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace Super.Model.Collections
{
	readonly ref struct Compiler<T>
	{
		readonly static ArrayPool<T> Pool = ArrayPool<T>.Shared;

		readonly T[] _source;

		public Compiler(in T[] source) => _source = source;

		public T[] Compile(in T[] first)
		{
			var count  = (uint)first.Length;
			var size   = count;
			var page   = 0u;
			var all    = new Page<ReadOnlyMemory<T>>(32) {[page++] = first};
			var length = _source.Length;
			while (count < length)
			{
				size        = Math.Min(int.MaxValue - size, size * 2);
				all[page++] = View(_source, size, ref count);
			}

			var       result      = new T[count];
			Memory<T> destination = result;
			var       current     = 0;
			for (var i = 0u; count > 0; i++)
			{
				var memory = all[i];
				var slots  = memory.Length;
				var slice  = destination.Slice(current, (int)Math.Min(count, slots));
				memory.CopyTo(slice);
				current += slots;
				count   -= (uint)slice.Length;
				Pool.Return(memory.Array());
			}

			all.Clear();
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static ReadOnlyMemory<T> View(in T[] parameter, in uint size, ref uint count)
		{
			var local  = 0u;
			var page   = new Page<T>(in size);
			var length = parameter.Length;
			var view   = page.Size;
			while (local < view && count < length)
			{
				page[local++] = parameter[count++];
			}

			return page.View(in local);
		}
	}

	sealed class Pager<T> : IInstance<T[], T[]>
	{
		public static Pager<T> Default { get; } = new Pager<T>();

		Pager() : this(1024) {}

		readonly uint _size;

		public Pager(in uint size) => _size = size;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T[] Get(in T[] parameter)
		{
			uint count = 0;

			var page   = new Page<T>(in _size);
			var length = parameter.Length;
			var size   = page.Size;
			while (count < size && count < length)
			{
				page[in count] = parameter[count++];
			}

			var first = page.Get(in count);
			return count >= length ? first : new Compiler<T>(in parameter).Compile(in first);
		}
	}
}
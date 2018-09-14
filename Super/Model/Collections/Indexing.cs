using System;
using System.Buffers;

namespace Super.Model.Collections
{
	readonly struct Indexing<T>
	{
		readonly static ArrayPool<T> Pool = ArrayPool<T>.Shared;

		readonly IIndex<uint, T> _source;

		public Indexing(IIndex<uint, T> source) => _source = source;

		public T[] Compile(T[] first)
		{
			var  total = (uint)first.Length;
			var  pages = 1u;
			var  all   = new Page<ReadOnlyMemory<T>>(32) {[0] = first};
			bool next;
			do
			{
				var size   = Math.Min(int.MaxValue - total, total * 2);
				var page   = new Page<T>(in size);
				var target = page.Size;
				var local  = 0u;
				while (local < target && _source.Next(in total, out var element))
				{
					page[local++] = element;
					total++;
				}

				all[pages++] = page.View(in local);
				next         = local == target;
			} while (next);

			var       result = new T[total];
			Memory<T> memory = result;
			var       index  = 0;
			for (var i = 0u; i < pages; i++)
			{
				var item        = all[i];
				var length      = (uint)item.Length;
				var destination = memory.Slice(index, (int)Math.Min(total, length));
				item.CopyTo(destination);
				index += destination.Length;
				total -= length;
				Pool.Return(item.Array());
			}

			all.Clear();
			return result;
		}
	}
}
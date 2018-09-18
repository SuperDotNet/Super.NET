using System;
using System.Collections.Generic;

namespace Super.Model.Collections
{
	readonly struct DynamicArray<T>
	{
		readonly ILease<T>      _lease;
		readonly ArrayView<T>[] _views;

		public DynamicArray(ILease<T> lease, ArrayView<T>[] views)
		{
			_lease = lease;
			_views = views;
		}

		public ArrayView<T> Get(IEnumerator<T> parameter)
		{
			var  array = _views;
			var  first = array[0];
			var  total = first.Length;
			var  pages = 1u;
			bool next;
			do
			{
				var size   = Math.Min(int.MaxValue - total, total * 2);
				var lease  = _lease.Get(size);
				var store  = lease.Array;
				var target = store.Length;
				var local  = 0u;
				while (local < target && parameter.MoveNext())
				{
					store[local++] = parameter.Current;
					total++;
				}

				array[pages++] = lease.Resize(local);
				next           = local == target;
			} while (next);

			var result = _lease.Get(total);
			var offset = 0u;
			var destination = result.Array;
			for (var i = 0u; i < pages; i++)
			{
				var segment = array[i];
				Array.ConstrainedCopy(segment.Array, 0, destination, (int)offset, (int)segment.Length);
				offset += segment.Length;
				segment.Dispose();
			}

			return result.Resize(offset);
		}
	}
}
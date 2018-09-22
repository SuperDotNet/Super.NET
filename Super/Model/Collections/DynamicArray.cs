using System;
using System.Collections.Generic;

namespace Super.Model.Collections
{
	readonly struct DynamicArray<T>
	{
		readonly IStores<T> _lease;
		readonly Store<T>[] _stores;
		readonly uint       _count;

		public DynamicArray(IStores<T> lease, Store<T>[] stores, uint count)
		{
			_lease  = lease;
			_stores = stores;
			_count  = count;
		}

		public Store<T> Get(IEnumerator<T> parameter)
		{
			var  total = _count;
			var  pages = 1u;
			bool next;
			do
			{
				var size   = Math.Min(int.MaxValue - total, total * 2);
				var lease  = _lease.Get(size);
				var store  = lease.Instance;
				var target = store.Length;
				var local  = 0u;
				while (local < target && parameter.MoveNext())
				{
					store[local++] = parameter.Current;
					total++;
				}

				_stores[pages++] = new Store<T>(store, local);
				next             = local == target;
			} while (next);

			var result      = _lease.Get(total);
			var offset      = 0u;
			var destination = result.Instance;
			for (var i = 0u; i < pages; i++)
			{
				var store = _stores[i].Copy(destination, offset);
				offset += store.Length;
				_lease.Execute(store.Instance);
			}

			return new Store<T>(destination, offset);
		}
	}
}
using System;
using System.Collections.Generic;

namespace Super.Model.Collections
{
	readonly struct DynamicArray<T>
	{
		readonly IStores<T> _stores;
		readonly Store<T>[] _store;

		public DynamicArray(IStores<T> stores, Store<T>[] store)
		{
			_stores = stores;
			_store  = store;
		}

		public ArrayView<T> Get(IEnumerator<T> parameter, in Selection selection)
		{
			var  total = selection.Start;
			var  pages = 1u;
			bool next;
			var length = selection.Length ?? int.MaxValue;
			do
			{
				var size   = Math.Min(int.MaxValue - total, total * 2);
				var lease  = _stores.Get(size);
				var store  = lease.Instance;
				var target = store.Length;
				var local  = 0u;
				while (local < target && total < length && parameter.MoveNext())
				{
					store[local++] = parameter.Current;
					total++;
				}

				_store[pages++] = new Store<T>(store, local);
				next            = local == target;
			} while (next);

			var result      = _stores.Get(total);
			var offset      = 0u;
			var destination = result.Instance;
			for (var i = 0u; i < pages; i++)
			{
				var store = _store[i].Copy(destination, offset);
				offset += store.Length;
				_stores.Execute(store.Instance);
			}

			return new ArrayView<T>(destination, 0, offset);
		}
	}
}
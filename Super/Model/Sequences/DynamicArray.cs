﻿using System;
using System.Collections.Generic;

namespace Super.Model.Sequences
{
	sealed class DynamicArray<T>
	{
		readonly static Allotted<Store<T>> Allotted = Allotted<Store<T>>.Default;

		readonly IStore<T>        _item;
		readonly IStore<Store<T>> _items;
		readonly Selection        _selection;

		public DynamicArray(IStore<T> item, in Selection selection) : this(item, Allotted, in selection) {}

		public DynamicArray(IStore<T> item, IStore<Store<T>> items, in Selection selection)
		{
			_item      = item;
			_items     = items;
			_selection = selection;
		}

		public ArrayView<T> Get(IEnumerator<T> parameter, in Store<T> first)
		{
			using (var session = _items.Session(32))
			{
				var  items  = session.Store.Instance;
				var  total  = _selection.Start;
				var  pages  = 1u;
				var  length = _selection.Length.Or(int.MaxValue);
				bool next;
				items[0] = first;
				do
				{
					var size   = Math.Min(int.MaxValue - total, total * 2);
					var lease  = _item.Get(size);
					var store  = lease.Instance;
					var target = store.Length;
					var local  = 0u;
					while (local < target && total < length && parameter.MoveNext())
					{
						store[local++] = parameter.Current;
						total++;
					}

					items[pages++] = new Store<T>(store, local);
					next           = local == target;
				} while (next);

				var result      = _item.Get(total);
				var offset      = 0u;
				var destination = result.Instance;
				for (var i = 0u; i < pages; i++)
				{
					using (var item = _item.Session(items[i]))
					{
						var amount = items[i].Length;
						Array.Copy(item.Store.Instance, 0, destination, offset, amount);
						offset += amount;
					}
				}

				return new ArrayView<T>(destination, 0, offset);
			}
		}
	}
}
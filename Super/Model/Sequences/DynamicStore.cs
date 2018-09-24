using Super.Model.Collections;
using System;
using System.Runtime.CompilerServices;

namespace Super.Model.Sequences
{
	sealed class DynamicIterator<T> : IIterator<T>
	{
		public static DynamicIterator<T> Default { get; } = new DynamicIterator<T>();

		DynamicIterator() : this(Allotted<T>.Default) {}

		readonly IStores<T> _stores;
		readonly uint       _size;

		public DynamicIterator(IStores<T> stores, uint size = 1024)
		{
			_stores = stores;
			_size   = size;
		}

		public T[] Get(IIteration<T> parameter)
		{
			var store = new DynamicStore<T>(_size);
			using (var session = _stores.Session(_size))
			{
				Store<T>? next = new Store<T>(session.Items, 0);
				while ((next = parameter.Get(next.Value)) != null)
				{
					store = store.Add(next.Value);
				}
			}

			return store.Get();
		}
	}


	readonly ref struct DynamicStore<T>
	{
		/*readonly static StoreReferences<T> References = StoreReferences<T>.Default;*/
		readonly static Allotted<T>        Item  = Allotted<T>.Default;
		readonly static Allotted<Store<T>> Items = Allotted<Store<T>>.Default;

		readonly Store<Store<T>>       _stores;
		readonly Collections.Selection _position;
		readonly uint                  _index;

		public DynamicStore(uint size, uint length = 32) : this(Items.Get(length), Collections.Selection.Default)
			=> _stores.Instance[0] = new Store<T>(Item.Get(size).Instance, 0);

		DynamicStore(Store<Store<T>> stores, Collections.Selection position, uint index = 0)
		{
			_stores   = stores;
			_position = position;
			_index    = index;
		}

		/*[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public DynamicStore<T> Add(in Store<T> page)
			=> new DynamicStore<T>(_stores, page.CopyInto(Size(page), _selection)
			                                      .Resize(page.Length));*/

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T[] Get()
		{
			var allocated = Allocated<T>.Default.Get(_position.Length ?? 0);
			using (Items.Session(_stores))
			{
				var stores = _stores.Instance;
				var total  = _index + 1;
				for (uint i = 0u, offset = 0u; i < total; i++)
				{
					var store = stores[i];
					using (Item.Session(store))
					{
						store.CopyInto(allocated, new Collections.Selection(offset, store.Length));
					}

					offset += store.Length;
				}
			}

			return allocated.Instance;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public DynamicStore<T> Add(in Store<T> page)
		{
			var stores   = _stores.Instance;
			var current  = stores[_index];
			var capacity = (uint)current.Instance.Length;
			var max      = _position.Start + capacity;
			var size     = page.Length;
			var filled   = size - (_position.Start + current.Length);

			if (size > max)
			{
				stores[_index] =
					new
						Store<T>(page.CopyInto(current, new Collections.Selection(current.Length, capacity - current.Length)).Instance,
						         capacity);

				var remainder = size - max;
				stores[_index + 1] =
					new Store<T>(page.CopyInto(Item.Get(Math.Min(int.MaxValue - size, size * 2)),
					                           new Collections.Selection(0, remainder))
					                 .Instance,
					             remainder);

				return new DynamicStore<T>(_stores, new Collections.Selection(max, size), _index + 1);
			}

			stores[_index] =
				new Store<T>(page.CopyInto(current, new Collections.Selection(current.Length, filled))
				                 .Instance,
				             current.Length + filled);
			return new DynamicStore<T>(_stores, new Collections.Selection(_position.Start, size), _index);
		}

		/*public Store<T> Get(IEnumerator<T> parameter, in Store<T> first)
		{
			using (var session = _items.Session(32))
			{
				var  items  = session.Items;
				var  total  = _selection.Start;
				var  pages  = 1u;
				var  length = _selection.Length ?? int.MaxValue;
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
						var amount = item.Store.Length;
						Array.Copy(items[i].Instance, 0u, destination, offset, amount);
						offset += amount;
					}
				}

				return new ArrayView<T>(destination, 0, offset);
			}
		}*/
	}
}
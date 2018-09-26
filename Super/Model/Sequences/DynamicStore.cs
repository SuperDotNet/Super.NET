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
				Store<T>? next = new Store<T>(session.Array, 0);
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
		readonly static Allotted<T>        Item  = Allotted<T>.Default;
		readonly static Allotted<Store<T>> Items = Allotted<Store<T>>.Default;

		readonly Store<T>[] _stores;
		readonly Collections.Selection _position;
		readonly uint                  _index;

		public DynamicStore(uint size, uint length = 32) : this(Items.Get(length), Collections.Selection.Default)
			=> _stores[0] = new Store<T>(Item.Get(size), 0);

		DynamicStore(Store<T>[] stores, Collections.Selection position, uint index = 0)
		{
			_stores   = stores;
			_position = position;
			_index    = index;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T[] Get()
		{
			var result = Allocated<T>.Default.Get(_position.Length ?? 0);
			using (Items.Session(_stores))
			{
				var total  = _index + 1;
				for (uint i = 0u, offset = 0u; i < total; i++)
				{
					var store = _stores[i];
					using (Item.Session(store.Instance))
					{
						store.CopyInto(result, new Collections.Selection(offset, store.Length));
					}

					offset += store.Length;
				}
			}

			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public DynamicStore<T> Add(in Store<T> page)
		{
			var stores   = _stores;
			var current  = stores[_index];
			var capacity = (uint)current.Instance.Length;
			var max      = _position.Start + capacity;
			var size     = page.Length;
			var filled   = size - (_position.Start + current.Length);

			if (size > max)
			{
				stores[_index] =
					new
						Store<T>(page.CopyInto(current.Instance, new Collections.Selection(current.Length, capacity - current.Length)),
						         capacity);

				var remainder = size - max;
				stores[_index + 1] =
					new Store<T>(page.CopyInto(Item.Get(Math.Min(int.MaxValue - size, size * 2)),
					                           new Collections.Selection(0, remainder)),
					             remainder);

				return new DynamicStore<T>(_stores, new Collections.Selection(max, size), _index + 1);
			}

			stores[_index] =
				new Store<T>(page.CopyInto(current.Instance, new Collections.Selection(current.Length, filled)),
				             current.Length + filled);
			return new DynamicStore<T>(_stores, new Collections.Selection(_position.Start, size), _index);
		}
	}
}
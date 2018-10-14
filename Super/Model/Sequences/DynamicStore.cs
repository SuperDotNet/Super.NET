using System;

namespace Super.Model.Sequences
{
	readonly ref struct DynamicStore<T>
	{
		readonly static Allotted<T>        Item  = Allotted<T>.Default;
		readonly static Allotted<Store<T>> Items = Allotted<Store<T>>.Default;

		readonly Store<T>[] _stores;
		readonly Selection  _position;
		readonly uint       _index;

		public DynamicStore(uint size, uint length = 32) : this(Items.Get(length).Instance,
		                                                        Selection.Default)
			=> _stores[0] = new Store<T>(Item.Get(size).Instance, 0);

		DynamicStore(Store<T>[] stores, Selection position, uint index = 0)
		{
			_stores   = stores;
			_position = position;
			_index    = index;
		}

		public T[] Get()
		{
			var allocated = Allocated<T>.Default.Get(_position.Length.Or(0));
			var result    = allocated.Instance;
			using (Items.Session(_stores))
			{
				var total = _index + 1;
				for (uint i = 0u, offset = 0u; i < total; i++)
				{
					var store = _stores[i];
					using (Item.Session(store.Instance))
					{
						store.Instance.CopyInto(result, new Selection(0, store.Length), offset);
					}

					offset += store.Length;
				}
			}

			return result;
		}

		public DynamicStore<T> Add(Store<T> page)
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
					new Store<T>(page.Instance.CopyInto(current.Instance,
					                                    new Selection(0, capacity - current.Length),
					                                    current.Length),
					             capacity);
				var remainder = size - max;
				stores[_index + 1] =
					new Store<T>(page.Instance.CopyInto(Item.Get(Math.Min(int.MaxValue - size, size * 2)).Instance,
					                                    new Selection(0, remainder)),
					             remainder);

				return new DynamicStore<T>(_stores, new Selection(max, size), _index + 1);
			}

			stores[_index]
				= new
					Store<T>(page.Instance.CopyInto(current.Instance, new Selection(0, filled), current.Length),
					         current.Length + filled);
			return new DynamicStore<T>(_stores, new Selection(_position.Start, size), _index);
		}
	}
}
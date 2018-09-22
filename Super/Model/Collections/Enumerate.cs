using Super.Model.Selection;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Super.Model.Collections
{
	public interface IEnumerate<T> : ISelect<IEnumerator<T>, Store<T>> {}

	sealed class Enumerate<T> : IEnumerate<T>
	{
		public static Enumerate<T> Default { get; } = new Enumerate<T>();

		Enumerate() : this(1024) {}

		readonly IStores<Store<T>> _items;
		readonly IStores<T>        _item;
		readonly uint              _size;

		public Enumerate(uint size = 1024) : this(Allotted<Store<T>>.Default, Allotted<T>.Default, size) {}

		public Enumerate(IStores<Store<T>> items, IStores<T> item, uint size = 1024)
		{
			_items = items;
			_item  = item;
			_size  = size;
		}

		static Store<T> Get(params T[] items) => new Store<T>(items);

		public Store<T> Get(IEnumerator<T> parameter)
		{
			if (!parameter.MoveNext())
			{
				return Store<T>.Empty;
			}

			var one = parameter.Current;

			if (!parameter.MoveNext())
			{
				return Get(one);
			}

			var two = parameter.Current;
			if (!parameter.MoveNext())
			{
				return Get(one, two);
			}

			var three = parameter.Current;
			if (!parameter.MoveNext())
			{
				return Get(one, two, three);
			}

			var four = parameter.Current;
			if (!parameter.MoveNext())
			{
				return Get(one, two, three, four);
			}

			var five = parameter.Current;

			var first = _item.Get(_size);
			var items = first.Instance;
			items[0] = one;
			items[1] = two;
			items[2] = three;
			items[3] = four;
			items[4] = five;
			var size  = items.Length;
			var count = 5u;
			while (count < size && parameter.MoveNext())
			{
				items[count++] = parameter.Current;
			}

			return count < size ? new Store<T>(items, count) : Compile(first, parameter);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		Store<T> Compile(in Store<T> first, IEnumerator<T> parameter)
		{
			var store    = _items.Get(32);
			var instance = store.Instance;
			instance[0] = first;
			var result = new DynamicArray<T>(_item, instance, first.Length).Get(parameter);
			_items.Execute(instance);
			return result;
		}
	}
}
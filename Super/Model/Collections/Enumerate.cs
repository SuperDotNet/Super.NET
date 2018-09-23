using Super.Model.Selection;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Super.Model.Collections
{
	public interface IEnumerate<T> : ISelect<IEnumerator<T>, ArrayView<T>> {}

	sealed class Enumerate<T> : IEnumerate<T>
	{
		public static Enumerate<T> Default { get; } = new Enumerate<T>();

		Enumerate() : this(Selection.Default) {}

		readonly IStores<Store<T>> _items;
		readonly IStores<T>        _item;
		readonly uint?             _skip;
		readonly Selection _target;

		public Enumerate(Selection selection, uint size = 1024)
			: this(Allotted<Store<T>>.Default, Allotted<T>.Default,
			       selection.Start == 0 ? (uint?)null : selection.Start, new Selection(size, selection.Length)) {}

		// ReSharper disable once TooManyDependencies
		public Enumerate(IStores<Store<T>> items, IStores<T> item, uint? skip, Selection target)
		{
			_items  = items;
			_item   = item;
			_skip   = skip;
			_target = target;
		}

		static ArrayView<T> Get(params T[] items) => new ArrayView<T>(items);

		public ArrayView<T> Get(IEnumerator<T> parameter)
		{
			if (_skip.HasValue)
			{
				for (var i = 0; i < _skip && parameter.MoveNext(); i++) {}
			}

			if (_target.Length == 0 || !parameter.MoveNext())
			{
				return ArrayView<T>.Empty;
			}

			var one = parameter.Current;

			if (_target.Length == 1 || !parameter.MoveNext())
			{
				return Get(one);
			}

			var two = parameter.Current;
			if (_target.Length == 2 || !parameter.MoveNext())
			{
				return Get(one, two);
			}

			var three = parameter.Current;
			if (_target.Length == 3 || !parameter.MoveNext())
			{
				return Get(one, two, three);
			}

			var four = parameter.Current;
			if (_target.Length == 4 || !parameter.MoveNext())
			{
				return Get(one, two, three, four);
			}

			var five = parameter.Current;

			var first = _item.Get(_target.Length.HasValue ? Math.Min(_target.Start, _target.Length.Value) : _target.Start);
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

			return count < size ? new ArrayView<T>(items, 0, count) : Compile(first, parameter);
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		ArrayView<T> Compile(in Store<T> first, IEnumerator<T> parameter)
		{
			var store    = _items.Get(32);
			var instance = store.Instance;
			instance[0] = first;
			var result = new DynamicArray<T>(_item, instance).Get(parameter, _target);
			_items.Execute(instance);
			return result;
		}
	}
}
using Super.Model.Commands;
using Super.Model.Selection;
using System.Buffers;
using System.Collections.Generic;

namespace Super.Model.Collections
{
	public interface IEnumerate<T> : ISelect<IEnumerator<T>, ArrayView<T>> {}

	sealed class Enumerate<T> : IEnumerate<T>
	{
		public static Enumerate<T> Default { get; } = new Enumerate<T>();

		Enumerate() : this(Lease<ArrayView<T>>.Default, Lease<T>.Default) {}

		readonly ILease<ArrayView<T>> _items;
		readonly ILease<T>            _item;
		readonly uint                 _size;

		public Enumerate(ILease<ArrayView<T>> items, ILease<T> item, uint size = 1024)
		{
			_items = items;
			_item  = item;
			_size  = size;
		}

		static ArrayView<T> Get(params T[] items) => new ArrayView<T>(items);

		public ArrayView<T> Get(IEnumerator<T> parameter)
		{
			if (!parameter.MoveNext())
			{
				return ArrayView<T>.Empty;
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

			var view  = _item.Get(_size);
			var items = view.Array;
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

			return count < size ? view.Resize(count) : Compile(view, parameter);
		}

		ArrayView<T> Compile(in ArrayView<T> first, IEnumerator<T> parameter)
		{
			var store = _items.Get(32);
			store.Array[0] = first;
			var result = new DynamicArray<T>(_item, store.Array).Get(parameter);
			_items.Execute(in store);
			return result;
		}
	}

	public interface ILease<T> : ISelect<uint, ArrayView<T>>, IEnhancedCommand<ArrayView<T>> {}

	sealed class Lease<T> : ILease<T>
	{
		public static Lease<T> Default { get; } = new Lease<T>();

		Lease() : this(ArrayPool<T>.Shared) {}

		readonly ArrayPool<T> _pool;

		public Lease(ArrayPool<T> pool) => _pool = pool;

		public ArrayView<T> Get(uint parameter) => new ArrayView<T>(_pool.Rent((int)parameter), 0, parameter);

		public void Execute(in ArrayView<T> parameter)
		{
			//_pool.Return(parameter.Array);
		}
	}

	sealed class Release<T> : ISelect<ArrayView<T>, Array<T>>
	{
		public static Release<T> Default { get; } = new Release<T>();

		Release() : this(Lease<T>.Default) {}

		readonly ILease<T> _lease;

		public Release(ILease<T> lease) => _lease = lease;

		public Array<T> Get(ArrayView<T> parameter)
		{
			var result = parameter.Get();
			if (result.Reference() != parameter.Array)
			{
				_lease.Execute(in parameter);
			}

			return result;
		}
	}
}
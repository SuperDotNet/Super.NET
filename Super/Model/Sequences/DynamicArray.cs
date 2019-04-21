using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Sequences.Query.Construction;
using System;
using System.Collections.Generic;

namespace Super.Model.Sequences
{
	public interface IEnumerate<T> : ISelect<IEnumerator<T>, Store<T>> {}

	public interface IStorage<T> : IStores<T>, ICommand<T[]> {}

	public sealed class DefaultStorage<T> : Storage<T>
	{
		public static DefaultStorage<T> Default { get; } = new DefaultStorage<T>();

		DefaultStorage() : base(Stores<T>.Default, EmptyCommand<T[]>.Default) {}
	}

	public sealed class Leases<T> : Storage<T>
	{
		public static Leases<T> Default { get; } = new Leases<T>();

		Leases() : base(Allotted<T>.Default, Return<T>.Default) {}
	}

	public interface IIterate<T> : ISelect<IEnumerable<T>, Store<T>> {}

	public sealed class Iterate<T> : IIterate<T>
	{
		public static Iterate<T> Default { get; } = new Iterate<T>();

		Iterate() : this(Enter<T>.Default, CollectionSelection<T>.Default, Enumerate<T>.Default) {}

		readonly IEnter<T>                         _enter;
		readonly ISelect<ICollection<T>, Store<T>> _collection;
		readonly IEnumerate<T>                     _enumerate;

		public Iterate(IEnter<T> enter, ISelect<ICollection<T>, Store<T>> collection, IEnumerate<T> enumerate)
		{
			_enter      = enter;
			_collection = collection;
			_enumerate  = enumerate;
		}

		public Store<T> Get(IEnumerable<T> parameter)
		{
			switch (parameter)
			{
				case T[] array:
					return _enter.Get(array);
				case ICollection<T> collection:
					return _collection.Get(collection);
				default:
					return _enumerate.Get(parameter.GetEnumerator());
			}
		}
	}

	public sealed class Enumerate<T> : IEnumerate<T>
	{
		public static Enumerate<T> Default { get; } = new Enumerate<T>();

		Enumerate() : this(Leases<Store<T>>.Default, Leases<T>.Default) {}

		readonly IStorage<Store<T>> _items;
		readonly IStorage<T>        _item;
		readonly uint               _start;

		public Enumerate(IStorage<Store<T>> items, IStorage<T> item, uint start = 8_192)
		{
			_items = items;
			_item  = item;
			_start = start;
		}

		public Store<T> Get(IEnumerator<T> parameter)
		{
			var  session = _items.Get(32);
			var  items   = session.Instance;
			var  marker  = _start;
			var  total   = 0u;
			var  pages   = 0u;
			bool next;
			do
			{
				var lease  = _item.Get(Math.Min(uint.MaxValue - marker, marker *= 2));
				var store  = lease.Instance;
				var target = store.Length;
				var local  = 0u;
				while (local < target && parameter.MoveNext())
				{
					store[local++] = parameter.Current;
					total++;
				}

				items[pages++] = new Store<T>(store, local);
				next           = local == target;
			} while (next);

			var result      = _item.Get(total);
			var destination = result.Instance;
			var offset      = 0u;
			for (var i = 0u; i < pages; i++)
			{
				var item     = items[i];
				var amount   = item.Length;
				var instance = item.Instance;
				Array.Copy(instance, 0, destination, offset, amount);
				offset += amount;
				_item.Execute(instance);
			}

			_items.Execute(items);
			return result;
		}
	}

	public class Storage<T> : Select<uint, Store<T>>, IStorage<T>
	{
		readonly ICommand<T[]> _return;

		public Storage(IStores<T> stores, ICommand<T[]> @return) : base(stores.Get) => _return = @return;

		public void Execute(T[] parameter)
		{
			_return.Execute(parameter);
		}
	}
}
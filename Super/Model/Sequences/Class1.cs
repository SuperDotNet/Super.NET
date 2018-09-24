using Super.Model.Collections;
using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Selection.Structure;
using System;

namespace Super.Model.Sequences
{
	public interface IIterator<T> : ISelect<IIteration<T>, T[]> {}

	public readonly struct Session<T> : IDisposable
	{
		readonly ICommand<T[]> _command;

		public Session(Store<T> store, ICommand<T[]> command)
		{
			Store    = store;
			_command = command;
		}

		public Store<T> Store { get; }

		public T[] Items => Store.Instance;

		public void Dispose()
		{
			_command.Execute(Store.Instance);
		}
	}

	static class Extensions
	{
		public static Session<T> Session<T>(this IStores<T> @this, uint amount) => @this.Session(@this.Get(amount));

		public static Session<T> Session<T>(this IStores<T> @this, Store<T> store) => new Session<T>(store, @this);
	}

	readonly ref struct ExpandingStore<T>
	{
		readonly IStores<T> _stores;
		readonly Store<T>   _store;

		public ExpandingStore(uint size) : this(Allotted<T>.Default, size) {}

		public ExpandingStore(IStores<T> stores, uint size)
			: this(stores, new Store<T>(stores.Get(size).Instance, 0)) {}

		ExpandingStore(IStores<T> stores, Store<T> store)
		{
			_stores = stores;
			_store  = store;
		}

		public ExpandingStore<T> Add(in Store<T> page)
		{
			var size   = Size(page);
			var store  = page.CopyInto(size, new Collections.Selection(_store.Length))
			                 .Resize(page.Length);
			var result = new ExpandingStore<T>(_stores, store);
			return result;
		}

		public Session<T> Session() => _stores.Session(_store);

		Store<T> Size(in Store<T> page)
		{
			var actual = (uint)_store.Instance.Length;
			var size   = page.Length;
			if (size > actual)
			{
				using (var session = Session())
				{
					return session.Store
					              .CopyInto(_stores.Get(Math.Min(int.MaxValue - size, size * 2)),
					                        new Collections.Selection(0, actual));
				}
			}

			return _store;
		}
	}

	sealed class Iterator<T> : IIterator<T>
	{
		public static Iterator<T> Default { get; } = new Iterator<T>();

		Iterator() : this(StoreReferences<T>.Default, Allotted<T>.Default) {}

		readonly IStoreReferences<T> _references;
		readonly IStores<T>          _stores;
		readonly uint                _size;

		public Iterator(IStoreReferences<T> references, IStores<T> stores, uint size = 1024)
		{
			_references = references;
			_stores     = stores;
			_size       = size;
		}

		public T[] Get(IIteration<T> parameter)
		{
			using (var session = _stores.Session(_size))
			{
				var store = new ExpandingStore<T>(_size);

				Store<T>? next = new Store<T>(session.Items, 0);
				while ((next = parameter.Get(next.Value)) != null)
				{
					store = store.Add(next.Value);
				}

				using (var @new = store.Session())
				{
					return _references.Get(@new.Store);
				}
			}
		}
	}

	public interface IIteration<T> : IStructure<Store<T>, Store<T>?> {}

	sealed class Iteration<T> : IIteration<T>
	{
		readonly T[]  _source;
		readonly uint _length;

		public Iteration(T[] source) : this(source, (uint)source.Length) {}

		public Iteration(T[] source, uint length)
		{
			_source = source;
			_length = length;
		}

		public Store<T>? Get(in Store<T> parameter)
		{
			var index = parameter.Length;
			if (index < _length)
			{
				var array   = parameter.Instance;
				var advance = (uint)Math.Min(index + array.Length, _length) - index;
				Array.Copy(_source, index,
				           array, 0, advance);

				return new Store<T>(array, index + advance);
			}

			return null;
		}
	}
}
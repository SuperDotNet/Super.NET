using Super.Model.Collections;
using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Selection.Structure;
using System;
using System.Runtime.CompilerServices;

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
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Session<T> Session<T>(this IStores<T> @this, uint amount) => @this.Session(@this.Get(amount));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Session<T> Session<T>(this IStores<T> @this, in Store<T> store) => new Session<T>(store, @this);
	}

	readonly ref struct ExpandingStore<T>
	{
		readonly static Allotted<T>        Stores     = Allotted<T>.Default;
		readonly static StoreReferences<T> References = StoreReferences<T>.Default;

		readonly Store<T>              _store;
		readonly Collections.Selection _selection;

		public ExpandingStore(uint size) : this(Stores, size) {}

		public ExpandingStore(IStores<T> stores, uint size) : this(new Store<T>(stores.Get(size).Instance, 0)) {}

		ExpandingStore(Store<T> store) : this(store, new Collections.Selection(store.Length)) {}

		ExpandingStore(Store<T> store, Collections.Selection selection)
		{
			_store     = store;
			_selection = selection;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ExpandingStore<T> Add(in Store<T> page)
			=> new ExpandingStore<T>(page.CopyInto(Size(page), _selection).Resize(page.Length));

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T[] Get()
		{
			using (Stores.Session(_store))
			{
				return References.Get(_store);
			}
		}

		/*[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Session<T> Session() => _stores.Session(_store);*/

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		Store<T> Size(in Store<T> page)
		{
			var actual = (uint)_store.Instance.Length;
			var size   = page.Length;
			if (size > actual)
			{
				using (var session = Stores.Session(_store))
				{
					return session.Store
					              .CopyInto(Stores.Get(Math.Min(int.MaxValue - size, size * 2)),
					                        new Collections.Selection(0, actual));
				}
			}

			return _store;
		}
	}

	sealed class Iterator<T> : IIterator<T>
	{
		public static Iterator<T> Default { get; } = new Iterator<T>();

		Iterator() : this(Allotted<T>.Default) {}

		readonly IStores<T> _stores;
		readonly uint       _size;

		public Iterator(IStores<T> stores, uint size = 1024)
		{
			_stores = stores;
			_size   = size;
		}

		public T[] Get(IIteration<T> parameter)
		{
			var store = new ExpandingStore<T>(_size);
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
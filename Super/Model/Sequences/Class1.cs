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

		public Session(T[] items, ICommand<T[]> command)
		{
			Items    = items;
			_command = command;
		}

		public T[] Items { get; }

		public void Dispose()
		{
			_command.Execute(Items);
		}
	}

	static class Extensions
	{
		public static Session<T> Session<T>(this IStores<T> @this, uint amount)
			=> new Session<T>(@this.Get(amount).Instance, @this);
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
				var       result = new Store<T>(_stores.Get(_size).Instance, 0);
				var       state  = new Store<T>(session.Items, 0);
				Store<T>? current;
				while ((current = parameter.Get(in state)) != null)
				{
					state = current.Value;
					var store = state.Into(Destination(result, state.Length), result.Length, _size).Instance;
					result = new Store<T>(store, state.Length);
				}

				return _references.Get(result);
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		Store<T> Destination(in Store<T> current, uint size)
		{
			var total = current.Instance.Length;
			if (size > total)
			{
				var array = _stores.Get(Math.Min(int.MaxValue - size, size * 2)).Instance;
				Array.Copy(current.Instance, array, total);
				_stores.Execute(current.Instance);
				return new Store<T>(array, current.Length);
			}

			return current;
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
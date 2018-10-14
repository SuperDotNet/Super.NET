using Super.Model.Selection;
using System;

namespace Super.Model.Sequences
{
	sealed class Iterator<T> : IIterator<T>
	{
		public static Iterator<T> Default { get; } = new Iterator<T>();

		Iterator() : this(Allotted<T>.Default) {}

		readonly IStore<T> _store;
		readonly uint      _size;

		public Iterator(IStore<T> store, uint size = 1024)
		{
			_store = store;
			_size  = size;
		}

		public T[] Get(IIteration<T> parameter)
		{
			var store = new DynamicStore<T>(_size);
			using (var session = _store.Session(_size))
			{
				Store<T>? next = new Store<T>(session.Store.Instance, 0);
				while ((next = parameter.Get(next.Value)) != null)
				{
					store = store.Add(next.Value);
				}
			}
			return store.Get();
		}
	}

	public interface IIteration<T> : ISelect<Store<T>, Store<T>?> {}

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

		public Store<T>? Get(Store<T> parameter)
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

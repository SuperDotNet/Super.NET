using Super.Model.Collections;
using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Selection.Structure;
using Super.Runtime.Activation;
using System.Buffers;

namespace Super.Model.Sequences
{
	public interface IStore<T> : ISelect<uint, Store<T>>, ICommand<T[]> {}

	sealed class Allocated<T> : DelegatedCommand<T[]>, IStore<T>
	{
		public static Allocated<T> Default { get; } = new Allocated<T>();

		Allocated() : base(Runtime.Delegates<T[]>.Empty) {}

		public Store<T> Get(uint parameter) => new T[parameter];
	}

	sealed class Allotted<T> : IStore<T>
	{
		public static Allotted<T> Default { get; } = new Allotted<T>();

		Allotted() : this(ArrayPool<T>.Shared) {}

		readonly ArrayPool<T> _pool;

		public Allotted(ArrayPool<T> pool) => _pool = pool;

		public Store<T> Get(uint parameter) => new Store<T>(_pool.Rent((int)parameter), parameter);

		public void Execute(T[] parameter)
		{
			_pool.Return(parameter);
		}
	}

	public interface ISelector<in TIn, out T> : ISelect<TIn, T[]> {}

	public interface IArraySelector<T> : IStructure<Store<T>, T[]> {}

	sealed class ArraySelector<T> : IArraySelector<T>, IActivateMarker<Collections.Selection>
	{
		public static ArraySelector<T> Default { get; } = new ArraySelector<T>();

		ArraySelector() : this(Collections.Selection.Default) {}

		readonly uint                 _start;
		readonly Assigned<uint>       _length;

		public ArraySelector(Collections.Selection selection) : this(selection.Start, selection.Length) {}

		public ArraySelector(uint start, Assigned<uint> length)
		{
			_start  = start;
			_length = length;
		}

		public T[] Get(in Store<T> parameter)
		{
			var size   = _length.IsAssigned ? _length.Instance : parameter.Length - _start;
			var result = parameter.Instance.CopyInto(new T[size], _start, size);
			return result;
		}
	}

	/*public readonly struct StoreView<T>
	{
		public StoreView(Store<T> store, uint start)
		{
			Store = store;
			Start  = start;
		}

		public Store<T> Store { get; }

		public uint Start { get; }
	}*/

	public readonly struct Store<T>
	{
		public static implicit operator Store<T>(T[] instance) => new Store<T>(instance, (uint)instance.Length);

		public static implicit operator ArrayView<T>(Store<T> store)
			=> new ArrayView<T>(store.Instance, 0, store.Length);

		public Store(T[] instance) : this(instance, (uint)instance.Length) {}

		public Store(T[] instance, uint length)
		{
			Instance = instance;
			Length   = length;
		}

		public T[] Instance { get; }

		public uint Length { get; }
	}
}
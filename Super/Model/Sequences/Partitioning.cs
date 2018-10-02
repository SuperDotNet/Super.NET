using Super.Model.Collections;
using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using System;
using System.Buffers;

namespace Super.Model.Sequences
{
	public interface IStore<T> : ISelect<uint, T[]>, ICommand<T[]> {}

	sealed class Allocated<T> : DelegatedCommand<T[]>, IStore<T>
	{
		public static Allocated<T> Default { get; } = new Allocated<T>();

		Allocated() : base(Runtime.Delegates<T[]>.Empty) {}

		public T[] Get(uint parameter) => new T[parameter];
	}

	sealed class Allotted<T> : IStore<T>
	{
		public static Allotted<T> Default { get; } = new Allotted<T>();

		Allotted() : this(ArrayPool<T>.Shared) {}

		readonly ArrayPool<T> _pool;

		public Allotted(ArrayPool<T> pool) => _pool = pool;

		public T[] Get(uint parameter) => _pool.Rent((int)parameter);

		public void Execute(T[] parameter)
		{
			_pool.Return(parameter);
		}
	}

	public interface IArraySelector<T> : IAlteration<T[]> {}

	sealed class ArraySelector<T> : IArraySelector<T>
	{
		public static ArraySelector<T> Default { get; } = new ArraySelector<T>();

		ArraySelector() : this(Collections.Selection.Default) {}

		readonly Func<uint, T[]>       _source;
		readonly Collections.Selection _selection;

		public ArraySelector(Collections.Selection selection) : this(Allocated<T>.Default, selection) {}

		public ArraySelector(IStore<T> store, Collections.Selection selection) : this(store.Get, selection) {}

		public ArraySelector(Func<uint, T[]> source, Collections.Selection selection)
		{
			_source    = source;
			_selection = selection;
		}

		public T[] Get(T[] parameter)
		{
			var size = _selection.Length.IsAssigned
				           ? _selection.Length.Instance
				           : (uint)parameter.Length - _selection.Start;
			return parameter.CopyInto(_source(size), _selection.Start, size);
		}
	}
}
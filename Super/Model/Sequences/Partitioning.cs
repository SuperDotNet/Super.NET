using Super.Model.Collections;
using Super.Model.Commands;
using Super.Model.Selection;
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

	public interface ISelector<T> : ISelector<T[], T> {}

	public interface ISelector<in TIn, out T> : ISelect<TIn, T[]> {}

	sealed class Selector<T> : ISelector<T>
	{
		public static Selector<T> Default { get; } = new Selector<T>();

		Selector() : this(Collections.Selection.Default) {}

		readonly Func<uint, T[]>       _source;
		readonly Collections.Selection _selection;

		public Selector(Collections.Selection selection) : this(Allocated<T>.Default.Get, selection) {}

		public Selector(Func<uint, T[]> source, Collections.Selection selection)
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
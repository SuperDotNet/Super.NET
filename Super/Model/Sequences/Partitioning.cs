using Super.Model.Collections;
using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using System.Buffers;

namespace Super.Model.Sequences
{
	public interface IStores<T> : ISelect<uint, T[]>, ICommand<T[]> {}

	sealed class Allocated<T> : IStores<T>
	{
		public static Allocated<T> Default { get; } = new Allocated<T>();

		Allocated() {}

		public T[] Get(uint parameter) => new T[parameter];

		public void Execute(T[] parameter) {}
	}

	sealed class Allotted<T> : IStores<T>
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

	sealed class SegmentedArray<T> : IAlteration<T[]>
	{
		readonly ISegment<T> _segment;
		readonly IStores<T>  _stores;
		readonly IStorage<T> _storage;

		public SegmentedArray(ISegment<T> segment, Collections.Selection selection)
			: this(segment, Allocated<T>.Default, selection) {}

		public SegmentedArray(ISegment<T> segment, IStores<T> stores, Collections.Selection selection)
			: this(segment, stores, new Storage<T>(selection)) {}

		public SegmentedArray(ISegment<T> segment, IStores<T> stores, IStorage<T> storage)
		{
			_segment = segment;
			_stores  = stores;
			_storage = storage;
		}

		public T[] Get(T[] parameter)
		{
			var storage = _storage.Get(parameter);
			using (var session = _stores.Session(storage.Instance))
			{
				var view   = _segment.Get(new ArrayView<T>(session.Array, 0, storage.Length));
				var result = _stores.New(view.Array, new Collections.Selection(view.Start, view.Length));
				return result;
			}
		}
	}

	public interface IStorage<T> : ISelect<T[], Store<T>> {}

	sealed class Storage<T> : IStorage<T>
	{
		readonly IClone<T>             _clone;
		readonly Collections.Selection _selection;

		public Storage(Collections.Selection selection) : this(new Clone<T>(selection), selection) {}

		public Storage(IClone<T> clone, Collections.Selection selection)
		{
			_clone     = clone;
			_selection = selection;
		}

		public Store<T> Get(T[] parameter) => new Store<T>(_clone.Get(parameter),
		                                                   _selection.Length ?? (uint)parameter.Length);
	}

	public interface IClone<T> : IAlteration<T[]> {}

	sealed class Clone<T> : IClone<T>
	{
		public static Clone<T> Default { get; } = new Clone<T>();

		Clone() : this(Collections.Selection.Default) {}

		readonly IStores<T>            _stores;
		readonly Collections.Selection _selection;

		public Clone(Collections.Selection selection) : this(Allotted<T>.Default, selection) {}

		public Clone(IStores<T> stores) : this(stores, Collections.Selection.Default) {}

		public Clone(IStores<T> stores, Collections.Selection selection)
		{
			_stores    = stores;
			_selection = selection;
		}

		public T[] Get(T[] parameter) => _stores.New(parameter, _selection);
	}
}
using Super.Model.Collections;
using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Model.Selection.Structure;
using System;
using System.Runtime.CompilerServices;

namespace Super.Model.Sequences
{
	public interface IArrays<T> : ISelect<uint, T[]>, ICommand<T[]> {}

	sealed class Allocated<T> : IArrays<T>
	{
		public static Allocated<T> Default { get; } = new Allocated<T>();

		Allocated() {}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T[] Get(uint parameter) => new T[parameter];

		public void Execute(T[] parameter) {}
	}

	sealed class PartitionReference<T> : Structure<Partition<T>, ArrayView<T>, T[]>
	{
		public static PartitionReference<T> Default { get; } = new PartitionReference<T>();

		PartitionReference() : base(Segment<T>.Default.Get, References<T>.Default.Get) {}
	}

	sealed class ArrayPartitions<T> : IAlteration<T[]>
	{
		public static ArrayPartitions<T> Default { get; } = new ArrayPartitions<T>();

		ArrayPartitions() : this(Allocated<T>.Default.Get, PartitionReference<T>.Default.Get,
		                         Collections.Selection.Default) {}

		readonly Func<uint, T[]>              _stores;
		readonly Selection<Partition<T>, T[]> _reference;
		readonly Collections.Selection        _selection;

		public ArrayPartitions(Func<uint, T[]> stores, Selection<Partition<T>, T[]> reference,
		                       Collections.Selection selection)
		{
			_stores    = stores;
			_reference = reference;
			_selection = selection;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public T[] Get(T[] parameter)
		{
			var length = _selection.Length ?? (uint)parameter.Length;

			var view = new ArrayView<T>(_stores(length), _selection.Start, length);

			Array.Copy(parameter, 0, view.Array, view.Start, view.Length);

			return _reference(new Partition<T>(new Store<T>(parameter), view));
		}
	}

	public interface ISegment<T> : IStructure<Partition<T>, ArrayView<T>> {}

	sealed class Segment<T> : ISegment<T>
	{
		public static Segment<T> Default { get; } = new Segment<T>();

		Segment() {}

		public ArrayView<T> Get(in Partition<T> parameter) => parameter.Destination;
	}

	/*sealed class SelectedSegment<T> : ISegment<T>
	{
		public static SelectedSegment<T> Default { get; } = new SelectedSegment<T>();

		SelectedSegment() : this(Collections.Selection.Default) {}

		readonly Collections.Selection _selection;

		public SelectedSegment(Collections.Selection selection) => _selection = selection;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public ArrayView<T> Get(in Partition<T> parameter)
		{
			var result = parameter.Destination.Resize(_selection.Start, _selection.Length ?? parameter.Source.Length);

			return result;
		}
	}*/

	public readonly struct Partition<T>
	{
		public Partition(Store<T> source, ArrayView<T> destination)
		{
			Source      = source;
			Destination = destination;
		}

		public Store<T> Source { get; }

		public ArrayView<T> Destination { get; }
	}
}
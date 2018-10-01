using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Selection.Structure;
using System.Buffers;
using System.Runtime.CompilerServices;

namespace Super.Model.Collections
{
	public interface ISegment<T> : ISegmentation<T, T> {}

	/*public interface ISegmentSelect<TIn, TOut> : IStructure<Segue<TIn, TOut>, ArrayView<TOut>> {}*/

	public interface ISegmentation<TIn, TOut> : IStructure<ArrayView<TIn>, ArrayView<TOut>> {}

	/*public readonly struct Segue<TFrom, TTo>
	{
		public Segue(ArrayView<TFrom> source, TTo[] destination)
		{
			Source      = source;
			Destination = destination;
		}

		public ArrayView<TFrom> Source { get; }

		public TTo[] Destination { get; }
	}*/

	public interface IStores<T> : ISelect<uint, Store<T>>, ICommand<T[]> {}

	sealed class Allocated<T> : IStores<T>
	{
		public static Allocated<T> Default { get; } = new Allocated<T>();

		Allocated() {}

		//[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Store<T> Get(uint parameter) => new Store<T>(new T[parameter]);

		public void Execute(T[] parameter) {}
	}

	sealed class Allotted<T> : IStores<T>
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

	public readonly struct Selection
	{
		public static Selection Default { get; } = new Selection(0);

		public static implicit operator Selection(Model.Assigned<uint> length) => new Selection(0, length);

		public Selection(uint start, Model.Assigned<uint> length = default)
		{
			Start  = start;
			Length = length;
		}

		public uint Start { get; }

		public Model.Assigned<uint> Length { get; }

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public Selection Resize(Model.Assigned<uint> size) => new Selection(Start, size);

		public static bool operator ==(Selection left, Selection right) => left.Equals(right);

		public static bool operator !=(Selection left, Selection right) => !left.Equals(right);

		bool Equals(Selection other) => Start == other.Start && Length == other.Length;

		public override bool Equals(object obj)
			=> !ReferenceEquals(null, obj) && obj is Selection other && Equals(other);

		public override int GetHashCode()
		{
			unchecked
			{
				return ((int)Start * 397) ^ Length.GetHashCode();
			}
		}
	}
}
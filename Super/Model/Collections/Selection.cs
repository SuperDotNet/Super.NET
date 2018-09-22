using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Selection.Structure;
using Super.Reflection;
using Super.Runtime.Activation;
using System;
using System.Buffers;

namespace Super.Model.Collections
{
	public interface ISegment<T> : ISegmentation<T, T> {}

	public interface ISegmentSelect<TIn, TOut> : IStructure<Segue<TIn, TOut>, ArrayView<TOut>> {}

	public interface ISegmentation<TIn, TOut> : IStructure<ArrayView<TIn>, ArrayView<TOut>> {}

	public readonly struct Segue<TFrom, TTo>
	{
		public Segue(ArrayView<TFrom> source, TTo[] destination)
		{
			Source      = source;
			Destination = destination;
		}

		public ArrayView<TFrom> Source { get; }

		public TTo[] Destination { get; }
	}

	/*public readonly struct Store<T>
	{
		public Store(T[] instance, uint requested)
		{
			Instance  = instance;
			Requested = requested;
		}

		public T[] Instance { get; }

		public uint Requested { get; }
	}*/

	public interface IStores<T> : ISelect<uint, T[]>, ICommand<T[]> {}

	sealed class Allocated<T> : IStores<T>
	{
		public static Allocated<T> Default { get; } = new Allocated<T>();

		Allocated() {}

		public T[] Get(uint parameter) => new T[parameter];

		public void Execute(T[] parameter) {}
	}

	sealed class Allotted<T> : DelegatedCommand<T[]>, IStores<T>
	{
		public static Allotted<T> Default { get; } = new Allotted<T>();

		Allotted() : this(ArrayPool<T>.Shared) {}

		readonly ArrayPool<T> _pool;

		public Allotted(ArrayPool<T> pool) : this(pool, I<Return<T>>.Default.From(pool).Execute) {}

		public Allotted(ArrayPool<T> pool, Action<T[]> complete) : base(complete) => _pool = pool;

		public T[] Get(uint parameter) => _pool.Rent((int)parameter);
	}

	sealed class Return<T> : ICommand<T[]>, IActivateMarker<ArrayPool<T>>
	{
		readonly ArrayPool<T> _pool;

		public Return(ArrayPool<T> pool) => _pool = pool;

		public void Execute(T[] parameter)
		{
			_pool.Return(parameter);
		}
	}

	public readonly struct Selection
	{
		public static Selection Default { get; } = new Selection(0, null);

		public Selection(uint start, uint? length)
		{
			Start  = start;
			Length = length;
		}

		public uint Start { get; }

		public uint? Length { get; }

		public static bool operator ==(Selection left, Selection right) => left.Equals(right);

		public static bool operator !=(Selection left, Selection right) => !left.Equals(right);

		public bool Equals(Selection other) => Start == other.Start && Length == other.Length;

		public override bool Equals(object obj)
			=> !ReferenceEquals(null, obj) && (obj is Selection other && Equals(other));

		public override int GetHashCode()
		{
			unchecked
			{
				return ((int)Start * 397) ^ Length.GetHashCode();
			}
		}
	}
}
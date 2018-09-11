using Super.Model.Selection;
using Super.Model.Selection.Stores;
using Super.Model.Sources;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Super.Model.Collections
{
	public interface IArray<T> : ISource<ReadOnlyMemory<T>> {}

	public class Array<T> : Source<ReadOnlyMemory<T>>, IArray<T>
	{
		public Array(IEnumerable<T> enumerable) : this(enumerable.ToArray()) {}

		public Array(params T[] instance) : base(instance) {}
	}

	public class DelegatedArray<T> : DelegatedSource<ReadOnlyMemory<T>>, IArray<T>
	{
		public DelegatedArray(Func<ReadOnlyMemory<T>> source) : base(source) {}
	}

	public class DecoratedArray<T> : DecoratedSource<ReadOnlyMemory<T>>, IArray<T>
	{
		public DecoratedArray(ISource<ReadOnlyMemory<T>> source) : base(source) {}
	}

	public interface IArray<in TFrom, TItem> : ISelect<TFrom, ReadOnlyMemory<TItem>> {}

	public class ArrayStore<TFrom, TTo> : Store<TFrom, ReadOnlyMemory<TTo>>, IArray<TFrom, TTo>
	{
		public ArrayStore(Func<TFrom, ReadOnlyMemory<TTo>> source) : base(source) {}
	}

	public class Array<TFrom, TItem> : IArray<TFrom, TItem>
	{
		readonly Func<TFrom, IEnumerable<TItem>> _selector;

		public Array(Func<TFrom, IEnumerable<TItem>> selector) => _selector = selector;

		public ReadOnlyMemory<TItem> Get(TFrom parameter) => _selector(parameter).ToArray();
	}

	public interface ISequence<out T> : ISource<IEnumerable<T>> {}

	public class Sequence<T> : Source<IEnumerable<T>>, ISequence<T>
	{
		public Sequence(IEnumerable<T> instance) : base(instance) {}
	}

	public class DecoratedSequence<T> : DecoratedSource<IEnumerable<T>>, ISequence<T>
	{
		public DecoratedSequence(ISource<IEnumerable<T>> instance) : base(instance) {}
	}

	public interface ISequence<in TFrom, out TItem> : ISelect<TFrom, IEnumerable<TItem>> {}

	sealed class Access<T> : Select<IEnumerable<T>, ReadOnlyMemory<T>>
	{
		public static Access<T> Default { get; } = new Access<T>();

		Access() : base(x => x.ToArray()) {}
	}

	static class Build
	{
		public static Builder<T> Array<T>(int maxCapacity = int.MaxValue) => new Builder<T>(maxCapacity);
	}

	struct Builder<T>
	{
		const int StartingCapacity = 4;

		readonly int          _maxCapacity;
		readonly ArrayPool<T> _pool;
		Buffer<T[]>           _buffers;

		T[] _current;

		int _index, _target, _count;

		public Builder(int maxCapacity) : this(maxCapacity, ArrayPool<T>.Shared) {}

		public Builder(int maxCapacity, ArrayPool<T> pool) : this()
		{
			_maxCapacity = maxCapacity;
			_pool        = pool;
			_buffers     = new Buffer<T[]>();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Add(T item)
		{
			if (_index >= _target)
			{
				AllocateBuffer();
			}

			_current[_index++] = item;

			_count++;
		}

		// ReSharper disable ComplexConditionExpression
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		void AllocateBuffer()
		{
			_buffers.Add(_current = _pool.Rent(Math.Min(_count == 0 ? StartingCapacity : _count, _maxCapacity - _count)));

			_index  = 0;
			_target = _current.Length;
		}

		public T[] ToArray()
		{
			var result    = new T[_count];
			var remaining = _count;
			var index     = 0;

			var length = _buffers.Count;
			for (var i = 0; i < length; i++)
			{
				var buffer = _buffers[i];

				var amount = Math.Min(remaining, buffer.Length);
				Array.Copy(buffer, 0, result, index, amount);
				_pool.Return(buffer);

				remaining -= amount;
				index     += amount;
			}

			_current = null;

			_buffers.Clear();

			return result;
		}
	}

	struct Buffer<T>
	{
		public void Clear() => _array = null;

		const int DefaultCapacity       = 4,
		          MaxCoreClrArrayLength = 0x7fefffff;

		T[] _array;

		uint _capacity;

		public uint Count;

		public T this[int index] => _array[index];

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Add(T item)
		{
			if (Count == _capacity)
			{
				_capacity = Capacity();
			}

			_array[Count++] = item;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		uint Capacity()
		{
			var result = Capacity(_capacity, Count + 1);
			var next   = new T[result];
			if (Count > 0)
			{
				Array.Copy(_array, 0, next, 0, Count);
			}

			_array = next;
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static uint Capacity(uint capacity, uint minimum)
		{
			var nextCapacity = capacity == 0 ? DefaultCapacity : 2 * capacity;

			if (nextCapacity > MaxCoreClrArrayLength)
			{
				nextCapacity = Math.Max(capacity + 1, MaxCoreClrArrayLength);
			}

			return Math.Max(nextCapacity, minimum);
		}
	}

	/*class Decorate<T> : ISequencer<T>
	{
		readonly ISequencer<T> _sequencer;

		public Decorate(ISequencer<T> sequencer) => _sequencer = sequencer;

		public ReadOnlyMemory<T> Get(IEnumerable<T> parameter) => _sequencer.Get(parameter);
	}*/

	/*sealed class Immutable<T>  : DecoratedSelect<ReadOnlyMemory<T>, ImmutableArray<T>>
	{
		public static Immutable<T> Default { get; } = new Immutable<T>();

		Immutable() : base(Enumerate<T>.Default.Select(x => x.ToImmutableArray())) {}
	}

	sealed class Enumerate<T> : Select<ReadOnlyMemory<T>, IEnumerable<T>>
	{
		public static Enumerate<T> Default { get; } = new Enumerate<T>();

		Enumerate() : base(x => x.AsEnumerable()) {}
	}*/
}
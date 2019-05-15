using Super.Model.Sequences.Query.Construction;
using System;
using System.Buffers;
using System.Collections.Generic;

namespace Super.Model.Sequences.Query
{
	sealed class DistinctDefinition<T> : Definition<T>
	{
		public static DistinctDefinition<T> Default { get; } = new DistinctDefinition<T>();

		DistinctDefinition() : this(EqualityComparer<T>.Default) {}

		public DistinctDefinition(IEqualityComparer<T> comparer) : base(Lease<T>.Default, new Distinct<T>(comparer)) {}
	}

	sealed class Default
	{
		public static ArrayPool<int> Numbers { get; } = ArrayPool<int>.Shared;
	}

	sealed class Distinct<T> : IProject<T>
	{
		readonly IEqualityComparer<T> _comparer;

		public Distinct(IEqualityComparer<T> comparer) => _comparer = comparer;

		public ArrayView<T> Get(ArrayView<T> parameter)
		{
			var from  = parameter.Start;
			var to    = from + parameter.Length;
			var array = parameter.Array;
			var count = 0u;
			var set   = new Set<T>(_comparer);
			for (var i = from; i < to; i++)
			{
				var item = array[i];
				if (set.Add(in item))
				{
					array[count++] = item;
				}
			}

			set.Clear();

			return new ArrayView<T>(array, 0, count);
		}
	}

	ref struct Set<T>
	{
		readonly static ArrayPool<Slot> Slots = ArrayPool<Slot>.Shared;

		readonly IEqualityComparer<T> comparer;
		int[]                         buckets;
		Slot[]                        slots;
		int                           count;
		int                           freeList;

		public Set(IEqualityComparer<T> comparer)
		{
			this.comparer = comparer;
			buckets       = Default.Numbers.Lease(7);
			slots         = Slots.Lease(7);
			freeList      = -1;
			count         = 0;
		}

		public void Clear()
		{
			Default.Numbers.Return(buckets);
			Slots.Return(slots);
		}

		public bool Add(in T value)
		{
			var hashCode = value?.GetHashCode() ?? 0;

			for (var index = buckets[hashCode % buckets.Length] - 1; index >= 0; index = slots[index].next)
			{
				if (slots[index].hashCode == hashCode && comparer.Equals(slots[index].value, value))
				{
					return false;
				}
			}

			int index1;
			if (freeList >= 0)
			{
				index1   = freeList;
				freeList = slots[index1].next;
			}
			else
			{
				if (count == slots.Length)
				{
					Resize();
				}

				index1 = count;
				++count;
			}

			var index2 = hashCode % buckets.Length;
			slots[index1].hashCode = hashCode;
			slots[index1].value    = value;
			slots[index1].next     = buckets[index2] - 1;
			buckets[index2]        = index1 + 1;

			return true;
		}

		void Resize()
		{
			var length    = checked(count * 2 + 1);
			var numArray  = Default.Numbers.Lease(length);
			var slotArray = Slots.Lease(length);
			Array.Copy(slots, 0, slotArray, 0, count);
			for (var index1 = 0; index1 < count; ++index1)
			{
				var index2 = slotArray[index1].hashCode % length;
				slotArray[index1].next = numArray[index2] - 1;
				numArray[index2]       = index1 + 1;
			}

			Clear();
			buckets = numArray;
			slots   = slotArray;
		}

		struct Slot
		{
			internal int hashCode, next;
			internal T   value;
		}
	}
}
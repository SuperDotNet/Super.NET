using Super.Model.Collections;
using Super.Model.Selection;
using Super.Runtime.Activation;
using System;
using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace Super.Runtime.Invocation
{
	/// <summary>
	/// Attribution: https://github.com/i3arnon/AsyncUtilities
	/// </summary>
	// ReSharper disable once PossibleInfiniteInheritance
	sealed class LockItem<T> : ISelect<int, (ImmutableArray<T> Items, int Mask)>
	{
		public static LockItem<T> Default { get; } = new LockItem<T>();

		LockItem() : this(New<T>.Default.ToDelegateReference()) {}

		readonly Func<T> _create;

		public LockItem(Func<T> create) => _create = create;

		public (ImmutableArray<T> Items, int Mask) Get(int parameter)
		{
			var mask   = GetStripeMask(parameter);
			var result = (new Repeat<T>(mask + 1, _create).ToImmutableArray(), mask);
			return result;
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		static int GetStripeMask(int stripes)
		{
			stripes |= stripes >> 1;
			stripes |= stripes >> 2;
			stripes |= stripes >> 4;
			stripes |= stripes >> 8;
			stripes |= stripes >> 16;
			return stripes;
		}
	}
}
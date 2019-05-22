using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Sequences;
using Super.Model.Sequences.Query;
using Super.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Super
{
	// ReSharper disable once MismatchedFileName
	public static partial class ExtensionMethods
	{
		public static ISelect<_, T[]> Open<_, T>(this ISelect<_, IEnumerable<T>> @this) => @this.Select(x => x.Open());

		public static ISelect<_, T[]> Open<_, T>(this ISelect<_, Array<T>> @this) => @this.Select(x => x.Open());

		public static ISelect<_, Array<T>> Result<_, T>(this ISelect<_, IEnumerable<T>> @this) => @this.Query().Get();

		public static IArray<_, T> ToStore<_, T>(this ISelect<_, Array<T>> @this) => @this.ToDelegate().ToStore();

		public static IArray<_, T> ToStore<_, T>(this Func<_, Array<T>> @this) => new ArrayStore<_, T>(@this);

		/**/

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static uint Length<T>(in this Model.Sequences.Store<T> @this)
			=> @this.Length.Or((uint)@this.Instance.Length);

		/**/

		public static ISelect<IEnumerable<TFrom>, IEnumerable<TTo>> Many<TFrom, TTo>(
			this ISelect<TFrom, IEnumerable<TTo>> @this) => new SelectManySelector<TFrom, TTo>(@this.Get);

		/**/

		public static ISequence<T> And<T>(this IResult<T> @this, params IResult<T>[] others)
			=> Sequence.From(others.Prepend(@this).ToArray());

		public static ISequence<T> And<T>(this T[] @this, params T[] others)
			=> Sequence.From(@this.Concat(others).ToArray());

		public static ISequence<T> And<T>(this T @this, params T[] others)
			=> Sequence.From(others.Prepend(@this).ToArray());

		/**/

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] ToArray<T>(in this ArrayView<T> @this)
			=> @this.Length == 0 ? Empty<T>.Array : @this.ToArray(new T[@this.Length]);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] ToArray<T>(in this ArrayView<T> @this, T[] into)
			=> @this.Array.CopyInto(into, @this.Start, @this.Length);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Model.Sequences.Query.Temp.Storage<T> ToStore<T>(in this ArrayView<T> @this)
			=> @this.ToStore(Model.Sequences.Query.Temp.Leases<T>.Default);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Model.Sequences.Query.Temp.Storage<T> ToStore<T>(in this ArrayView<T> @this, Model.Sequences.Query.Temp.IStores<T> stores)
		{
			var result = stores.Get(@this.Length);
			@this.ToArray(result.Instance);
			return result;
		}

		// ReSharper disable once TooManyArguments
		public static T[] CopyInto<T>(this T[] @this, T[] result, in Selection selection, uint offset = 0)
			=> @this.CopyInto(result, selection.Start, selection.Length.IsAssigned
				                                           ? selection.Length.Instance
				                                           : (uint)result.Length - offset, offset);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] CopyInto<T>(this T[] @this, T[] result) => @this.CopyInto(result, 0u, (uint)@this.Length);

		// ReSharper disable once TooManyArguments
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] CopyInto<T>(this T[] @this, T[] result, uint start, uint length, uint offset = 0)
		{
			if (length < 32)
			{
				for (var i = 0; i < length; i++)
				{
					result[i + offset] = @this[start + i];
				}
			}
			else
			{
				Array.Copy(@this, start,
				           result, offset, length);
			}
			return result;
		}

		public static ArrayView<T> Resize<T>(in this ArrayView<T> @this, uint size) => @this.Resize(@this.Start, size);

		public static ArrayView<T> Resize<T>(in this ArrayView<T> @this, uint start, uint size)
			=> start != @this.Start || size != @this.Length
				   ? new ArrayView<T>(@this.Array, start, size)
				   : @this;
	}
}
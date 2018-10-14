using Super.Model.Selection;
using Super.Model.Sequences;
using Super.Model.Sequences.Query;
using Super.Reflection;
using Super.Runtime;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Super
{
	// ReSharper disable once MismatchedFileName
	public static partial class ExtensionMethods
	{
		public static ISelect<_, T[]> Fixed<_, T>(this ISelect<_, IEnumerable<T>> @this)
			=> @this.Select(x => x.Fixed());

		public static ISelect<_, Array<T>> Result<_, T>(this ISelect<_, IEnumerable<T>> @this)
			=> @this.Select(Model.Sequences.Result<T>.Default);

		public static ISelect<_, IEnumerable<T>> Reference<_, T>(this ISelect<_, Array<T>> @this)
			=> @this.Select(x => x.Reference());

		public static ISelect<_, ArrayView<T>> View<_, T>(this ISelect<_, T[]> @this)
			=> @this.Select(View<T>.Default);

		public static ISelect<_, T[]> Fixed<_, T>(this ISelect<_, Array<T>> @this) => @this.Reference().Fixed();

		public static IArray<_, T> ToStore<_, T>(this ISelect<_, Array<T>> @this) => @this.ToDelegate().ToStore();

		public static IArray<_, T> ToStore<_, T>(this Func<_, Array<T>> @this) => new ArrayStore<_, T>(@this);

		/**/

		public static ISequence<_, T> Sequence<_, T>(this ISelect<_, Array<T>> @this)
			=> @this.Fixed().Sequence();

		public static ISequence<_, T> Sequence<_, T>(this ISelect<_, T[]> @this)
			=> new ArraySequence<_, T>(@this.Select(x => new Store<T>(x)));

		public static ISequence<_, T> Sequence<_, T>(this ISelect<_, ICollection<T>> @this)
			=> new CollectionSequence<_, T>(@this);

		public static ISequence<_, T> Select<_, T>(this ISequence<_, T> @this, Selection selection)
			=> @this.Skip(selection.Start)
			        .Take(selection.Length);

		public static ISequence<_, T> Skip<_, T>(this ISequence<_, T> @this, uint skip) => @this.Get(new Skip(skip));

		public static ISequence<_, T> Take<_, T>(this ISequence<_, T> @this, uint take) => @this.Get(new Take(take));

		public static ISequence<_, T> WhereBy<_, T>(this ISequence<_, T> @this, Expression<Func<T, bool>> where)
			=> @this.Where(where.Compile());

		public static ISequence<_, T> Where<_, T>(this ISequence<_, T> @this, Func<T, bool> where)
			=> @this.Get(new Where<T>(where));

		public static ISelect<_, T> Get<_, T>(this ISequence<_, T> @this, IElement<T> element) => @this.Get(element);

		public static ISelect<_, T> FirstAssigned<_, T>(this ISequence<_, T> @this)
			where T : class => @this.Get(FirstAssigned<T>.Default);

		public static ISelect<_, T?> FirstAssigned<_, T>(this ISequence<_, T?> @this)
			where T : struct => @this.Get(FirstAssignedValue<T>.Default);

		public static ISelect<_, T> Only<_, T>(this ISequence<_, T> @this)
			=> @this.Get(Model.Sequences.Query.Only<T>.Default);

		public static ISelect<_, T> Only<_, T>(this ISequence<_, T> @this, Func<T, bool> where)
			=> where.To(I<Only<T>>.Default).To(@this.Get);

		public static ISelect<_, T> Single<_, T>(this ISequence<_, T> @this) => @this.Get(Single<T>.Default);

		public static ISelect<_, T> Single<_, T>(this ISequence<_, T> @this, Func<T, bool> where)
			=> where.To(I<Single<T>>.Default).To(@this.Get);

		public static ISelect<_, Array<T>> Result<_, T>(this ISequence<_, T> @this) => @this.Get().Result();

		/**/

		// TODO: remove.

		public static ISelect<IEnumerable<TFrom>, IEnumerable<TTo>> Select<TFrom, TTo>(this ISelect<TFrom, TTo> @this)
			=> @this.ToDelegate().To(I<SelectSelector<TFrom, TTo>>.Default);

		public static ISelect<IEnumerable<TFrom>, IEnumerable<TTo>> SelectMany<TFrom, TTo>(
			this ISelect<TFrom, IEnumerable<TTo>> @this)
			=> @this.ToDelegate().To(I<SelectManySelector<TFrom, TTo>>.Default);

		/**/

		public static ISelect<TIn, TOut[]> Yield<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> @this.Select(Model.Sequences.Query.Yield<TOut>.Default);

		/**/

		public static Session<T> Session<T>(this IStore<T> @this, uint amount)
			=> new Session<T>(@this.Get(amount), @this);

		public static Session<T> Session<T>(this IStore<T> @this, T[] items) => new Session<T>(items, @this);

		public static Session<T> Session<T>(this IStore<T> @this, in Store<T> store)
			=> new Session<T>(store.Instance, @this);

		public static ISelectView<T> Continue<T>(this ISelect<ArrayView<T>, Session<T>> @this,
		                                         ISelect<ArrayView<T>, ArrayView<T>> @continue)
			=> new SessionSelection<T>(@this.Get, @continue.Get);

		/**/

		public static ISelect<_, Store<T>> Store<_, T>(this ISelect<_, ArrayView<T>> @this)
			=> @this.Select(x => new Store<T>(x.Array, x.Length));

		public static T[] ToArray<T>(in this ArrayView<T> @this)
			=> @this.Length == 0 ? Empty<T>.Array : @this.ToArray(new T[@this.Length]);

		public static T[] ToArray<T>(in this ArrayView<T> @this, T[] into)
			=> @this.Array.CopyInto(into, @this.Start, @this.Length);

		// ReSharper disable once TooManyArguments
		public static T[] CopyInto<T>(this T[] @this, T[] result, in Selection selection, uint offset = 0)
			=> @this.CopyInto(result, selection.Start, selection.Length.IsAssigned
				                                           ? selection.Length.Instance
				                                           : (uint)result.Length - offset, offset);

		// ReSharper disable once TooManyArguments
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T[] CopyInto<T>(this T[] @this, T[] result, uint start, uint length, uint offset = 0)
		{
			Array.Copy(@this, start,
			           result, offset, length);
			return result;
		}

		// ReSharper disable once TooManyArguments
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Store<T> CopyInto<T>(this T[] @this, in Store<T> store, uint start, uint offset = 0)
		{
			Array.Copy(@this, start, store.Instance, offset, store.Length - offset);
			return store;
		}

		public static ArrayView<T> Resize<T>(in this ArrayView<T> @this, uint size) => @this.Resize(@this.Start, size);

		public static ArrayView<T> Resize<T>(in this ArrayView<T> @this, uint start, uint size)
		{
			var index  = start;
			var length = size;
			return index != @this.Start || length != @this.Length
				       ? new ArrayView<T>(@this.Array, index, length)
				       : @this;
		}
	}
}
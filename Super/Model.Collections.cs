using Super.Model.Selection;
using Super.Model.Sequences;
using Super.Model.Sequences.Query;
using Super.Reflection;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

// ReSharper disable TooManyArguments

namespace Super
{
	// ReSharper disable once MismatchedFileName

	public static partial class ExtensionMethods
	{
		public static IReadOnlyDictionary<TKey, Array<TValue>>
			ToDictionary<TKey, TValue>(this IEnumerable<IGrouping<TKey, TValue>> @this)
			=> @this.ToDictionary(x => x.Key, x => new Array<TValue>(x.ToArray()));

		public static ISelect<_, IReadOnlyDictionary<TKey, Array<T>>>
			Grouping<_, T, TKey>(this ISelect<_, Array<T>> @this, ISelect<T, TKey> select)
			=> @this.Grouping(select.Get);

		public static ISelect<_, IReadOnlyDictionary<TKey, Array<T>>>
			Grouping<_, T, TKey>(this ISelect<_, Array<T>> @this, Func<T, TKey> select)
			=> @this.Select(new Grouping<T, TKey>(select));

		public static ISelect<_, T[]> Fixed<_, T>(this ISelect<_, IEnumerable<T>> @this)
			=> @this.Select(x => x.Fixed());

		public static ISelect<_, Array<T>> Result<_, T>(this ISelect<_, IEnumerable<T>> @this)
			=> @this.Select(Model.Sequences.Result<T>.Default);

		/*public static ISelect<_, Array<T>> Result<_, T>(this ISelect<_, T[]> @this)
			=> @this.Select(Model.Sequences.Result<T>.Default);*/

		public static ISelect<_, IEnumerable<T>> Reference<_, T>(this ISelect<_, Array<T>> @this)
			=> @this.Select(x => x.Reference());

		public static ISelect<_, T[]> Instance<_, T>(this ISelect<_, Array<T>> @this) => @this.Reference().Fixed();

		public static IArray<_, T> ToStore<_, T>(this ISelect<_, Array<T>> @this) => @this.ToDelegate().ToStore();

		public static IArray<_, T> ToStore<_, T>(this Func<_, Array<T>> @this) => new ArrayStore<_, T>(@this);

		/**/

		// TODO: remove.

		public static ISelect<IEnumerable<TFrom>, IEnumerable<TTo>> Select<TFrom, TTo>(this ISelect<TFrom, TTo> @this)
			=> @this.ToDelegate().To(I<SelectSelector<TFrom, TTo>>.Default);

		public static ISelect<IEnumerable<TFrom>, IEnumerable<TTo>> SelectMany<TFrom, TTo>(
			this ISelect<TFrom, IEnumerable<TTo>> @this)
			=> @this.ToDelegate().To(I<SelectManySelector<TFrom, TTo>>.Default);

		/**/

		public static ISelect<TIn, TOut> FirstAssigned<TIn, TOut>(this ISelect<TIn, Array<TOut>> @this)
			where TOut : class => @this.Select(FirstAssigned<TOut>.Default);

		public static ISelect<TIn, TOut?> FirstAssigned<TIn, TOut>(this ISelect<TIn, Array<TOut?>> @this)
			where TOut : struct => @this.Select(FirstAssignedValue<TOut>.Default);

		public static ISelect<TIn, TOut> Only<TIn, TOut>(this ISelect<TIn, Array<TOut>> @this)
			=> @this.Select(Model.Sequences.Query.Only<TOut>.Default);

		public static ISelect<TIn, TOut> Only<TIn, TOut>(this ISelect<TIn, Array<TOut>> @this,
		                                                 Func<TOut, bool> where)
			=> where.To(I<Only<TOut>>.Default).To(@this.Select);

		public static ISelect<TIn, TOut> Single<TIn, TOut>(this ISelect<TIn, Array<TOut>> @this)
			=> @this.Select(Single<TOut>.Default);

		public static ISelect<TIn, TOut> Single<TIn, TOut>(this ISelect<TIn, Array<TOut>> @this,
		                                                   Func<TOut, bool> where)
			=> where.To(I<Single<TOut>>.Default).To(@this.Select);

		public static ISelect<TIn, TOut[]> Yield<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> @this.Select(Model.Sequences.Query.Yield<TOut>.Default);

		public static ISelect<_, ImmutableArray<T>> Emit<_, T>(this ISelect<_, IEnumerable<T>> @this)
			=> @this.Select(Immutable<T>.Default);
	}
}
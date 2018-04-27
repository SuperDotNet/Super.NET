﻿using Super.Model.Collections;
using Super.Model.Selection;
using Super.Reflection;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Super
{
	public static partial class ExtensionMethods
	{
		public static ISelect<IEnumerable<TFrom>, IEnumerable<TTo>> SelectMany<TFrom, TTo>(this ISelect<TFrom, IEnumerable<TTo>> @this)
			=> @this.ToDelegate().To(I<SelectManySelector<TFrom, TTo>>.Default);

		public static ISelect<IEnumerable<TFrom>, IEnumerable<TTo>> Select<TFrom, TTo>(this ISelect<TFrom, TTo> @this)
			=> @this.ToDelegate().To(I<SelectSelector<TFrom, TTo>>.Default);

		public static IEnumerable<T> AsEnumerable<T>(this ImmutableArray<T> @this)
			=> EnumerableSelector<T>.Default.Get(@this);

		/*public static ISelect<TParameter, TResult> Composite<TParameter, TResult>(
			this IEnumerable<ISelect<TParameter, TResult>> @this)
			=> @this.Fixed().To(x => x.Skip(1).Aggregate(x.First(), (current, alteration) => alteration.Or(current)));*/

		public static ISelect<TIn, TOut> FirstAssigned<TIn, TOut>(this ISelect<TIn, IEnumerable<TOut>> @this)
			=> @this.Select(FirstOrDefaultSelector<TOut>.Default);

		public static ISelect<TIn, ImmutableArray<TOut>> Enumerate<TIn, TOut>(this ISelect<TIn, IEnumerable<TOut>> @this)
			=> @this.Select(ImmutableArraySelector<TOut>.Default);

		public static ISelect<TIn, IEnumerable<TOut>> Hide<TIn, TOut>(this ISelect<TIn, ImmutableArray<TOut>> @this)
			=> @this.Select(EnumerableSelector<TOut>.Default);

		public static ISelect<TIn, IEnumerable<TOut>> Sequence<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> @this.Select(YieldSelector<TOut>.Default);
	}
}
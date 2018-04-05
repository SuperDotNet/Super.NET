using Super.Model.Collections;
using Super.Model.Sources;
using Super.Reflection;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Super.ExtensionMethods
{
	partial class Sources
	{
		public static ISource<ImmutableArray<TParameter>, TResult> Enumerate<TParameter, TResult>(
			this ISource<IEnumerable<TParameter>, TResult> @this)
			=> @this.In(EnumerableSelector<TParameter>.Default);

		public static ISource<TParameter, ImmutableArray<TResult>> Enumerate<TParameter, TResult>(
			this ISource<TParameter, IEnumerable<TResult>> @this)
			=> @this.Out(ImmutableArraySelector<TResult>.Default);

		public static ISource<IEnumerable<TParameter>, TResult> AsEnumerable<TParameter, TResult>(
			this ISource<ImmutableArray<TParameter>, TResult> @this)
			=> @this.In(ImmutableArraySelector<TParameter>.Default);

		public static ISource<TParameter, IEnumerable<TResult>> AsEnumerable<TParameter, TResult>(
			this ISource<TParameter, ImmutableArray<TResult>> @this)
			=> @this.Out(EnumerableSelector<TResult>.Default);

		public static ISource<ImmutableArray<TTo>, TResult> In<TFrom, TTo, TResult>(
			this ISource<ImmutableArray<TFrom>, TResult> @this, ISource<TTo, TFrom> coercer)
			=> @this.AsEnumerable().In(coercer.ToDelegate()).Enumerate();

		public static ISource<IEnumerable<TTo>, TResult> In<TFrom, TTo, TResult>(
			this ISource<IEnumerable<TFrom>, TResult> @this, ISource<TTo, TFrom> coercer)
			=> @this.In(coercer.ToDelegate());

		public static ISource<IEnumerable<TFrom>, TResult> In<TResult, TFrom, TTo>(
			this ISource<IEnumerable<TTo>, TResult> @this, Func<TFrom, TTo> coercer)
			=> @this.In(new SelectCoercer<TFrom, TTo>(coercer));

		public static ISource<TParameter, IEnumerable<TTo>> Out<TParameter, TFrom, TTo>(
			this ISource<TParameter, IEnumerable<TFrom>> @this, ISource<TFrom, IEnumerable<TTo>> select)
			=> @this.Out(select.ToDelegate());

		public static ISource<TParameter, IEnumerable<TTo>> Out<TParameter, TFrom, TTo>(
			this ISource<TParameter, IEnumerable<TFrom>> @this, Func<TFrom, IEnumerable<TTo>> select)
			=> @this.Out(new SelectManyCoercer<TFrom, TTo>(select));

		public static ISource<TParameter, IEnumerable<TTo>> Out<TParameter, TFrom, TTo>(
			this ISource<TParameter, IEnumerable<TFrom>> @this, ISource<TFrom, TTo> select)
			=> @this.Out(select.ToDelegate());

		public static ISource<TParameter, IEnumerable<TTo>> Out<TParameter, TFrom, TTo>(
			this ISource<TParameter, IEnumerable<TFrom>> @this, Func<TFrom, TTo> select)
			=> @this.Out(new SelectCoercer<TFrom, TTo>(select));
	}
}
﻿using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Runtime;
using Super.Runtime.Activation;
using System;

namespace Super
{
	public static partial class ExtensionMethods
	{
		public static ISource<T> AsSource<T>(this ISource<T> @this) => @this;

		public static ISource<TTo> Select<TFrom, TTo>(this ISource<TFrom> @this, ISelect<TTo> select)
			=> @this.Select(select.Out<TFrom>());

		public static ISource<TTo> Select<TFrom, TTo>(this ISource<TFrom> @this, ISelect<TFrom, TTo> select)
			=> @this.Select(select.Get);

		public static ISource<T> Select<T>(this ISource<ISource<T>> @this) => @this.Select(ValueSelector<T>.Default);

		public static ISelect<TParameter, TResult> Select<TParameter, TResult>(
			this ISource<ISelect<TParameter, TResult>> @this)
			=> I<DelegatedInstanceSelector<TParameter, TResult>>.Default.From(@this);

		public static ISource<TTo> Select<TFrom, TTo>(this ISource<TFrom> @this, Func<TFrom, TTo> select)
			=> new DelegatedSelection<TFrom, TTo>(select, @this.Get);

		public static ISource<TResult> Select<TParameter, TResult>(this ISelect<TParameter, TResult> @this,
		                                                           TParameter parameter)
			=> new FixedSelection<TParameter, TResult>(@this, parameter);

		public static ISource<TResult> Select<TParameter, TResult>(this ISelect<TParameter, TResult> @this,
		                                                           ISource<TParameter> parameter)
			=> @this.Select(parameter.Get);

		public static ISource<TResult> Select<TParameter, TResult>(this ISelect<TParameter, TResult> @this,
		                                                           Func<TParameter> parameter)
			=> new DelegatedSelection<TParameter, TResult>(@this.Get, parameter);

		public static ISource<TResult> Select<TParameter, TResult>(this ISource<TParameter> @this, I<TResult> _)
			where TResult : IActivateMarker<TParameter>
			=> @this.Select(MarkedActivations<TParameter, TResult>.Default);

		public static TResult Get<TParameter, TResult>(this ISource<TParameter> @this,
		                                               ISelect<TParameter, TResult> select) => @this.Get(select.Get);

		public static TTo Get<TFrom, TTo>(this ISource<TFrom> @this, Func<TFrom, TTo> select)
			=> @this.Select(select).Get();

		public static ISelect<TParameter, TResult> Or<TParameter, TResult>(this ISource<TResult> @this,
		                                                                   ISelect<TParameter, TResult> select)
			=> Allow(@this, I<TParameter>.Default).Or(select);

		public static ISource<T> Or<T>(this ISource<T> @this, ISource<T> fallback)
			=> @this.Or(IsAssigned<T>.Default, fallback);

		public static ISource<T> Or<T>(this ISource<T> @this, ISpecification<T> specification, ISource<T> fallback)
			=> new ValidatedSource<T>(specification, @this, fallback);

		public static ISpecification<TParameter, TResult> Pair<TParameter, TResult>(
			this TResult @this, ISpecification<TParameter> specification)
			=> @this.ToSelect(I<TParameter>.Default).ToSpecification(specification);

		public static ISelect<object, T> Allow<T>(this ISource<T> @this) => Allow(@this, I<object>.Default);

		public static ISelect<TParameter, TResult> Allow<TParameter, TResult>(this ISource<TResult> @this, I<TParameter> _)
			=> new DelegatedResult<TParameter, TResult>(@this.Get);

		public static ISource<T> Singleton<T>(this ISource<T> @this) => SingletonSelector<T>.Default.Get(@this);

		public static ISource<T> ToSource<T>(this T @this) => Sources<T>.Default.Get(@this);

		/*public static ISource<T> Source<T>(this IMutable<T> @this) => @this;*/

		public static ISource<T> ToSource<T>(this Func<T> @this) => I<DelegatedSource<T>>.Default.From(@this);

		public static Func<T> ToDelegate<T>(this ISource<T> @this) => @this.Get;

		public static Func<T> ToDelegateReference<T>(this ISource<T> @this) => Model.Sources.Delegates<T>.Default.Get(@this);
	}
}
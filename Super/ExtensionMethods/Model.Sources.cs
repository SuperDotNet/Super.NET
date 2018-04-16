using Super.Model.Selection;
using Super.Model.Selection.Alterations;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Runtime.Activation;
using System;
using System.Reactive;

namespace Super.ExtensionMethods
{
	partial class Model
	{
		public static ISource<TTo> Select<TFrom, TTo>(this ISource<TFrom> @this, ISelect<TTo> select)
			=> @this.Select(select.Out<TFrom>());

		public static ISource<TTo> Select<TFrom, TTo>(this ISource<TFrom> @this, ISelect<TFrom, TTo> select)
			=> @this.Select(select.ToDelegate());

		public static ISource<T> Select<T>(this ISource<ISource<T>> @this) => @this.Select(ValueSelector<T>.Default);

		public static ISelect<TParameter, TResult> Select<TParameter, TResult>(this ISource<ISelect<TParameter, TResult>> @this)
			=> I<DelegatedInstanceSelector<TParameter, TResult>>.Default.From(@this);

		public static ISource<TTo> Select<TFrom, TTo>(this ISource<TFrom> @this, Func<TFrom, TTo> select)
			=> new DelegatedSelection<TFrom, TTo>(select, @this.ToDelegate());

		public static ISource<TResult> Select<TParameter, TResult>(this ISelect<TParameter, TResult> @this,
		                                                           TParameter parameter)
			=> new FixedSelection<TParameter, TResult>(@this, parameter);

		public static ISelect<TParameter, TResult> Or<TParameter, TResult>(this ISource<TResult> @this,
		                                                                   ISelect<TParameter, TResult> @select)
			=> @this.Allow(I<TParameter>.Default).Or(@select);

		public static ISource<T> Or<T>(this ISource<T> @this, ISource<T> fallback)
			=> @this.Or(IsAssigned<T>.Default, fallback);

		public static ISource<T> Or<T>(this ISource<T> @this, ISpecification<T> specification, ISource<T> fallback)
			=> new ValidatedSource<T>(specification, @this, fallback);

		public static ISource<TResult> Select<TParameter, TResult>(this ISource<TParameter> @this, I<TResult> _)
			where TResult : IActivateMarker<TParameter>
			=> @this.Select(Activations<TParameter, TResult>.Default);

		public static TTo Get<TFrom, TTo>(this ISource<TFrom> @this, Func<TFrom, TTo> select)
			=> @this.Select(select).Get();

		public static TResult Get<TParameter, TResult>(this ISource<TParameter> @this,
		                                               ISelect<TParameter, TResult> @select)
			=> @this.Get(@select.ToDelegate());

		public static TResult Any<TParameter, TResult>(this ISource<TResult> @this, TParameter _) => @this.Get();

		public static ISelect<object, T> Any<T>(this ISource<T> @this) => @this.Allow(I<object>.Default);
		public static ISelect<Unit, T> In<T>(this ISource<T> @this) => @this.Allow(I<Unit>.Default);

		public static ISpecification<TParameter, TResult> Allow<TParameter, TResult>(
			this TResult @this, ISpecification<TParameter> specification)
			=> @this.ToSource(I<TParameter>.Default).ToSpecification(specification);

		public static ISelect<TParameter, TResult> Allow<TParameter, TResult>(this TResult @this, I<TParameter> infer)
			=> @this.ToInstance().Allow(infer);

		public static ISelect<TParameter, TResult> Allow<TParameter, TResult>(this ISource<TResult> @this, I<TParameter> _)
			=> new DelegatedResult<TParameter, TResult>(@this.ToDelegate());

		public static ISource<T> Singleton<T>(this ISource<T> @this) => SingletonSelector<T>.Default.Get(@this);

		public static ISource<T> ToInstance<T>(this T @this) => Sources<T>.Default.Get(@this);

		public static ISource<T> ToInstance<T>(this Func<T> @this) => I<DelegatedSource<T>>.Default.From(@this);

		public static Func<T> ToDelegate<T>(this ISource<T> @this) => Super.Model.Sources.Delegates<T>.Default.Get(@this);
	}
}
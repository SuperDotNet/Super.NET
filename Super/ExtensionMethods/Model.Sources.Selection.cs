using Super.Model.Containers;
using Super.Model.Sources;
using System;

// ReSharper disable TooManyArguments

namespace Super.ExtensionMethods
{
	public static partial class Model
	{
		public static ISource<TFrom, TResult> In<TFrom, TTo, TResult>(this ISource<TTo, TResult> @this,
		                                                              ISelect<TFrom> select)
			=> @this.In(select.In<TTo>());

		public static ISource<TFrom, TResult> In<TFrom, TTo, TResult>(this ISource<TTo, TResult> @this,
		                                                              ISource<TFrom, TTo> select)
			=> @this.In(select.ToDelegate());

		public static ISource<TFrom, TResult> In<TFrom, TTo, TResult>(this ISource<TTo, TResult> @this,
		                                                              Func<TFrom, TTo> select)
			=> new SelectedParameterSource<TTo, TFrom, TResult>(@this.ToDelegate(), select);

		public static ISource<TParameter, TTo> Out<TParameter, TFrom, TTo>(this ISource<TParameter, TFrom> @this,
		                                                                   ISelect<TTo> select)
			=> @this.Out(select.Out<TFrom>());

		public static ISource<TParameter, TTo> Out<TParameter, TFrom, TTo>(this ISource<TParameter, TFrom> @this,
		                                                                   ISource<TFrom, TTo> select)
			=> @this.Out(select.ToDelegate());

		public static ISource<TParameter, TTo> Out<TParameter, TFrom, TTo>(this ISource<TParameter, TFrom> @this,
		                                                                   Func<TFrom, TTo> select)
			=> new SelectedResult<TParameter, TFrom, TTo>(@this.ToDelegate(), select);

		public static ISpecification<TFrom, TResult> In<TFrom, TTo, TResult>(
			this ISpecification<TTo, TResult> @this, ISource<TFrom, TTo> coercer)
			=> new SpecificationSource<TFrom, TResult>(@this.Select(coercer), @this.To<ISource<TTo, TResult>>().In(coercer));

		public static ISource<TParameter, TResult> Into<TParameter, TResult>(
			this ISource<TParameter, TResult> @this, ISource<Decoration<TParameter, TResult>, TResult> other)
			=> new Decorator<TParameter, TResult>(other, @this);
	}
}
using Super.Model.Selection;
using System;

// ReSharper disable TooManyArguments

namespace Super.ExtensionMethods
{
	public static partial class Model
	{
		public static ISelect<TFrom, TResult> In<TFrom, TTo, TResult>(this ISelect<TTo, TResult> @this,
		                                                              ISelect<TFrom> select)
			=> @this.In(select.In<TTo>());

		public static ISelect<TFrom, TResult> In<TFrom, TTo, TResult>(this ISelect<TTo, TResult> @this,
		                                                              ISelect<TFrom, TTo> select)
			=> @this.In(select.ToDelegate());

		public static ISelect<TFrom, TResult> In<TFrom, TTo, TResult>(this ISelect<TTo, TResult> @this,
		                                                              Func<TFrom, TTo> select)
			=> new Parameter<TTo, TFrom, TResult>(@this.ToDelegate(), select);

		public static ISpecification<TFrom, TResult> In<TFrom, TTo, TResult>(
			this ISpecification<TTo, TResult> @this, ISelect<TFrom, TTo> coercer)
			=> @this.ToSelect().ToSource().In(coercer).ToSpecification(@this.Select(coercer));

		public static ISelect<TParameter, TResult> Into<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISelect<Decoration<TParameter, TResult>, TResult> other)
			=> new Decorator<TParameter, TResult>(other, @this);

		public static ISelect<TParameter, TTo> Out<TParameter, TFrom, TTo>(this ISelect<TParameter, TFrom> @this,
		                                                                   ISelect<TTo> select)
			=> @this.Out(select.Out<TFrom>());

		public static ISelect<TParameter, TTo> Out<TParameter, TFrom, TTo>(this ISelect<TParameter, TFrom> @this,
		                                                                   ISelect<TFrom, TTo> select)
			=> @this.Out(select.ToDelegate());

		public static ISelect<TParameter, TTo> Out<TParameter, TFrom, TTo>(this ISelect<TParameter, TFrom> @this,
		                                                                   Func<TFrom, TTo> select)
			=> new Result<TParameter, TFrom, TTo>(@this.ToDelegate(), select);
	}
}
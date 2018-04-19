using Super.Model.Selection;
using System;

// ReSharper disable TooManyArguments

namespace Super
{
	public static partial class ExtensionMethods
	{
		public static ISelect<TFrom, TResult> In<TFrom, TTo, TResult>(this ISelect<TTo, TResult> @this,
		                                                              ISelect<TFrom> select)
			=> @this.In(select.In<TTo>());

		public static ISelect<TFrom, TResult> In<TFrom, TTo, TResult>(this ISelect<TTo, TResult> @this,
		                                                              ISelect<TFrom, TTo> select)
			=> In(@this, ToDelegate(@select));

		public static ISelect<TFrom, TResult> In<TFrom, TTo, TResult>(this ISelect<TTo, TResult> @this,
		                                                              Func<TFrom, TTo> select)
			=> new Parameter<TTo, TFrom, TResult>(ToDelegate(@this), select);

		public static ISpecification<TFrom, TResult> In<TFrom, TTo, TResult>(
			this ISpecification<TTo, TResult> @this, ISelect<TFrom, TTo> coercer)
			=> ToSpecification(ToSelect(@this).ToSelect().In(coercer), @this.Select(coercer));

		public static ISelect<TParameter, TResult> Into<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISelect<Decoration<TParameter, TResult>, TResult> other)
			=> new Decorator<TParameter, TResult>(other, @this);

		public static ISelect<TParameter, TTo> Out<TParameter, TFrom, TTo>(this ISelect<TParameter, TFrom> @this,
		                                                                   ISelect<TTo> select)
			=> @this.Out(select.Out<TFrom>());

		public static ISelect<TParameter, TTo> Out<TParameter, TFrom, TTo>(this ISelect<TParameter, TFrom> @this,
		                                                                   ISelect<TFrom, TTo> select)
			=> Out(@this, ToDelegate(@select));

		public static ISelect<TParameter, TTo> Out<TParameter, TFrom, TTo>(this ISelect<TParameter, TFrom> @this,
		                                                                   Func<TFrom, TTo> select)
			=> new Result<TParameter, TFrom, TTo>(ToDelegate(@this), select);
	}
}
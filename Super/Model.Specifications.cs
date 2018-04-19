using Super.Model.Selection;
using Super.Model.Specifications;
using System;

namespace Super
{
	public static partial class ExtensionMethods
	{
		public static ISpecification<T> ToSpecification<T>(this Func<T, bool> @this) => Specifications<T>.Default.Get(@this);

		public static ISpecification<T> Select<T>(this ISpecification<T> @this, T parameter)
			=> @this.IsSatisfiedBy(parameter) ? Always<T>.Default : Never<T>.Default;

		public static ISpecification<TFrom> Select<TFrom, TTo>(this ISpecification<TTo> @this, ISelect<TFrom> select)
			=> @this.Select(select.In<TTo>());

		public static ISpecification<TFrom> Select<TFrom, TTo>(this ISpecification<TTo> @this, ISelect<TFrom, TTo> select)
			=> @this.Select(ToDelegate(@select));

		public static ISpecification<TFrom> Select<TFrom, TTo>(this ISpecification<TTo> @this, Func<TFrom, TTo> select)
			=> new SelectedParameterSpecification<TFrom,TTo>(@this.ToDelegate(), select);

		public static Func<T, bool> ToDelegate<T>(this ISpecification<T> @this) => Delegates<T>.Default.Get(@this);

		public static ISelect<TParameter, TResult> If<TParameter, TResult>(
			this ISpecification<TParameter> @this, ISelect<TParameter, TResult> @true) => If(@this, @true, Default(@true));

		public static ISelect<TParameter, TResult> If<TParameter, TResult>(
			this ISpecification<TParameter> @this, ISelect<TParameter, TResult> @true, ISelect<TParameter, TResult> @false)
			=> new DelegatedConditional<TParameter, TResult>(@this, @true, @false);

		public static ISpecification<T> Or<T>(this ISpecification<T> @this, params ISpecification<T>[] others)
			=> new AnySpecification<T>(others.Prepend(@this).Fixed());

		public static ISpecification<T> And<T>(this ISpecification<T> @this, params ISpecification<T>[] others)
			=> new AllSpecification<T>(others.Prepend(@this).Fixed());

		public static ISpecification<T> Inverse<T>(this ISpecification<T> @this)
			=> InverseSpecifications<T>.Default.Get(@this);

		public static ISpecification<T> Equal<T>(this T @this) => EqualitySpecifications<T>.Default.Get(@this);

		public static ISpecification<T> Not<T>(this T @this) => Inverse(@this.Equal());
	}
}
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Runtime;
using System;

namespace Super.ExtensionMethods
{
	public static class Specifications
	{
		/*public static ISpecification<T> ToSpecification<T>(this ISource<T, bool> @this)
			=> @this.ToDelegate().ToSpecification();

		public static ISpecification<T> ToSpecification<T>(this Func<T, bool> @this) => Specifications<T>.Default.Get(@this);

		public static ISpecification<object> ToSpecification(this IInstance<bool> @this) => @this.ToDelegate().ToSpecification();

		public static ISpecification<object> ToSpecification(this Func<bool> @this) => @this.To(I<FixedDelegatedSpecification<object>>.Default);*/

		/*public static ISource<TParameter, ISource<T, bool>> Adapt<TParameter, T>(
			this ISource<TParameter, ISpecification<T>> @this)
			=> @this.Out(SpecificationSourceCoercer<T>.Default);

		public static ISpecification<TTo> Adapt<TFrom, TTo>(this ISpecification<TFrom> @this, ISource<TTo, TFrom> coercer)
			=> @this.Adapt(coercer.ToDelegate());*/

		/*public static ISpecification<TTo> Adapt<TFrom, TTo>(this ISpecification<TFrom> @this, Func<TTo, TFrom> coercer)
			=> @this.Adapt().In(coercer).ToSpecification();*/

		/*public static ISource<T, bool> Adapt<T>(this ISpecification<T> @this) => Model.Specifications.Adapters<T>.Default.Get(@this);*/

		public static ISpecification<TFrom> Select<TFrom, TTo>(this ISpecification<TTo> @this, ISource<TFrom, TTo> select)
			=> @this.Select(select.ToDelegate());

		public static ISpecification<TFrom> InTo<TFrom, TResult>(this ISpecification<Type> @this, ISource<TFrom, TResult> source)
			=> @this.Select(InstanceTypeCoercer<TFrom>.Default);

		public static ISpecification<TFrom> Select<TFrom, TTo>(this ISpecification<TTo> @this, Func<TFrom, TTo> select)
			=> new SelectedParameterSpecification<TFrom,TTo>(@this.ToDelegate(), select);

		public static Func<T, bool> ToDelegate<T>(this ISpecification<T> @this) => Delegates<T>.Default.Get(@this);

		public static ISource<TParameter, TResult> If<TParameter, TResult>(
			this ISpecification<TParameter> @this, ISource<TParameter, TResult> @true) => @this.If(@true, @true.Default());

		public static ISource<TParameter, TResult> If<TParameter, TResult>(
			this ISpecification<TParameter> @this, ISource<TParameter, TResult> @true, ISource<TParameter, TResult> @false)
			=> new Conditional<TParameter, TResult>(@this, @true, @false);

		public static ISpecification<T> Or<T>(this ISpecification<T> @this, params ISpecification<T>[] others)
			=> new AnySpecification<T>(others.Prepend(@this).Fixed());

		public static ISpecification<T> And<T>(this ISpecification<T> @this, params ISpecification<T>[] others)
			=> new AllSpecification<T>(others.Prepend(@this).Fixed());

		public static ISpecification<T> Inverse<T>(this ISpecification<T> @this)
			=> InverseSpecifications<T>.Default.Get(@this);

		public static ISpecification<T> Equal<T>(this T @this) => EqualitySpecifications<T>.Default.Get(@this);

		public static ISpecification<T> Not<T>(this T @this) => @this.Equal().Inverse();
	}
}
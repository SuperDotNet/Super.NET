using System;
using Super.Model.Sources;
using Super.Model.Sources.Coercion;
using Super.Model.Specifications;

namespace Super.ExtensionMethods
{
	public static class Specifications
	{
		public static ISpecification<T> ToSpecification<T>(this ISource<T, bool> @this)
			=> @this.ToDelegate().ToSpecification();

		public static ISpecification<T> ToSpecification<T>(this Func<T, bool> @this) => Specifications<T>.Default.Get(@this);

		public static ISource<TParameter, ISource<T, bool>> Adapt<TParameter, T>(
			this ISource<TParameter, ISpecification<T>> @this)
			=> @this.Out(SpecificationSourceCoercer<T>.Default);

		public static ISource<T, bool> Adapt<T>(this ISpecification<T> @this)
			=> Model.Specifications.Adapters<T>.Default.Get(@this);

		public static ISource<TParameter, TResult> If<TParameter, TResult>(
			this ISpecification<TParameter> @this, ISource<TParameter, TResult> @true)
			=> @this.If(@true, Defaults<TParameter, TResult>.Default.Get(@true));

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
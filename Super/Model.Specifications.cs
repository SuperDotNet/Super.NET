using Super.Model.Selection;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Runtime.Activation;
using System;
using System.Reactive;

namespace Super
{
	public static partial class ExtensionMethods
	{
		public static ISpecification<T> AsSpecification<T>(this ISpecification<T> @this) => @this;

		public static Func<T, bool> ToDelegate<T>(this ISpecification<T> @this) => @this.IsSatisfiedBy;
		public static Func<T, bool> ToDelegateReference<T>(this ISpecification<T> @this) => Delegates<T>.Default.Get(@this);

		public static bool IsSatisfiedBy(this ISpecification<Unit> @this) => @this.IsSatisfiedBy(Unit.Default);

		public static IAny Any(this ISpecification<Unit> @this) => I<Any>.Default.From(@this);

		public static ISpecification<T> ToSpecification<T>(this ISelect<T, bool> @this) => @this.ToDelegateReference().ToSpecification();
		public static ISpecification<T> ToSpecification<T>(this Func<T, bool> @this) => Specifications<T>.Default.Get(@this);

		/*public static ISpecification<TypeInfo> Select<T>(this ISpecification<TypeInfo> @this) => @this.Select(Reflection.Types.Type<T>.Metadata);*/

		/*public static IAny Select<T>(this ISpecification<T> @this, ISource<T> parameter)
			=> new DelegatedResultSpecification(parameter.Select(@this.IsSatisfiedBy).Get).Any();*/

		/*public static ISpecification<T> Select<T>(this ISpecification<T> @this, T parameter)
			=> @this.IsSatisfiedBy(parameter) ? Always<T>.Default : Never<T>.Default;*/

		/*public static ISpecification<TFrom> Select<TFrom, TTo>(this ISpecification<TTo> @this, ISelect<TFrom> select)
			=> @this.Select(select.In<TTo>());

		public static ISpecification<TFrom> Select<TFrom, TTo>(this ISpecification<TTo> @this, ISelect<TFrom, TTo> select)
			=> @this.Select(select.ToDelegate());

		public static ISpecification<TFrom> Select<TFrom, TTo>(this ISpecification<TTo> @this, Func<TFrom, TTo> select)
			=> new SelectedParameterSpecification<TFrom,TTo>(@this.IsSatisfiedBy, select);*/

		public static ISelect<TParameter, TResult> If<TParameter, TResult>(
			this ISpecification<TParameter> @this, ISelect<TParameter, TResult> @true) => @this.If(@true, @true.Default());

		public static ISelect<TParameter, TResult> If<TParameter, TResult>(
			this ISpecification<TParameter> @this, ISelect<TParameter, TResult> @true, ISelect<TParameter, TResult> @false)
			=> new Validated<TParameter, TResult>(@this, @true, @false);

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
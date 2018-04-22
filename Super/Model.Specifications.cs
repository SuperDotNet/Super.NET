using Super.Model.Selection;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Reflection.Types;
using Super.Runtime.Activation;
using System;
using System.Reactive;
using System.Reflection;

namespace Super
{
	public static partial class ExtensionMethods
	{
		/*public static IAny Specification(this ISpecification @this) => @this;*/

		public static bool IsSatisfiedBy(this ISpecification @this, object _) => @this.IsSatisfiedBy();
		public static bool IsSatisfiedBy(this ISpecification @this) => @this.IsSatisfiedBy(Unit.Default);

		public static ISpecification<T> Allow<T>(this ISpecification @this) => @this.Select<T, Unit>(_ => Unit.Default);
		public static IAny Any(this ISpecification @this) => I<Any>.Default.From(@this);

		public static ISpecification<T> ToSpecification<T>(this ISelect<T, bool> @this) => @this.ToDelegate().ToSpecification();
		public static ISpecification<T> ToSpecification<T>(this Func<T, bool> @this) => Specifications<T>.Default.Get(@this);

		public static ISpecification<TypeInfo> Select<T>(this ISpecification<TypeInfo> @this) => @this.Select(Type<T>.Metadata);

		public static IAny Select<T>(this ISpecification<T> @this, ISource<T> parameter)
			=> new DelegatedResultSpecification(parameter.Select(@this.ToDelegate()).ToDelegate()).Any();

		public static ISpecification<T> Select<T>(this ISpecification<T> @this, T parameter)
			=> @this.IsSatisfiedBy(parameter) ? Always<T>.Default : Never<T>.Default;

		public static ISpecification<TFrom> Select<TFrom, TTo>(this ISpecification<TTo> @this, ISelect<TFrom> select)
			=> @this.Select(select.In<TTo>());

		public static ISpecification<TFrom> Select<TFrom, TTo>(this ISpecification<TTo> @this, ISelect<TFrom, TTo> select)
			=> @this.Select(select.ToDelegate());

		public static ISpecification<TFrom> Select<TFrom, TTo>(this ISpecification<TTo> @this, Func<TFrom, TTo> select)
			=> new SelectedParameterSpecification<TFrom,TTo>(@this.ToDelegate(), select);

		public static ISpecification<T> OncePerParameter<T>(this ISpecification<T> @this) => OnceAlteration<T>.Default.Get(@this);
		public static ISpecification<T> OnlyOnce<T>(this ISpecification<T> @this) => OnlyOnceAlteration<T>.Default.Get(@this);

		public static Func<T, bool> ToDelegate<T>(this ISpecification<T> @this) => Model.Specifications.Delegates<T>.Default.Get(@this);

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
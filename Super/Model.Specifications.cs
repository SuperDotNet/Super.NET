using Super.Model.Selection;
using Super.Model.Specifications;
using Super.Reflection;
using System;
using System.Reactive;

namespace Super
{
	// ReSharper disable once MismatchedFileName

	public static partial class ExtensionMethods
	{
		public static ISpecification<T> AsSpecification<T>(this ISpecification<T> @this) => @this;

		public static Func<T, bool> ToDelegate<T>(this ISpecification<T> @this) => @this.IsSatisfiedBy;
		public static Func<T, bool> ToDelegateReference<T>(this ISpecification<T> @this) => Delegates<T>.Default.Get(@this);
		
		public static bool IsSatisfiedBy(this ISpecification<Unit> @this) => @this.IsSatisfiedBy(Unit.Default);

		public static IAny Any(this ISpecification<Unit> @this) => I<Any>.Default.From(@this);

		/*public static ISpecification<T> ToSpecification<T>(this ISelect<T, bool> @this) => @this.ToDelegateReference().ToSpecification();
		public static ISpecification<T> ToSpecification<T>(this Func<T, bool> @this) => Specifications<T>.Default.Get(@this);*/

		public static ISpecification<T> Or<T>(this ISpecification<T> @this, params ISpecification<T>[] others)
			=> new AnySpecification<T>(others.Prepend(@this).Fixed());

		public static ISpecification<T> And<T>(this ISpecification<T> @this, params ISpecification<T>[] others)
			=> new AllSpecification<T>(others.Prepend(@this).Fixed());

		public static ISpecification<T> Inverse<T>(this ISpecification<T> @this)
			=> InverseSpecifications<T>.Default.Get(@this);

		public static ISpecification<T> Equal<T>(this T @this) => EqualitySpecifications<T>.Default.Get(@this);

		public static ISpecification<T> Not<T>(this T @this) => @this.Equal().Inverse();

		public static ISelect<T, bool> Out<T>(this ISpecification<T> @this) => new Select<T, bool>(@this.IsSatisfiedBy);

		public static IAny Out<T>(this ISpecification<T> @this, T parameter) => @this.Out().Out(parameter);

		public static ISpecification<T, TOut> Out<T, TOut>(this ISpecification<T> @this, ISelect<T, TOut> parameter)
			=> new Specification<T, TOut>(@this, parameter);
	}
}
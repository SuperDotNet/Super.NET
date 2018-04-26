using Super.Model.Selection;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Runtime;
using System;
using System.Reactive;

namespace Super
{
	public static partial class ExtensionMethods
	{
		public static ISource<T> AsSource<T>(this ISource<T> @this) => @this;

		public static ISelect<TParameter, TResult> Or<TParameter, TResult>(this ISource<TResult> @this,
		                                                                   ISelect<TParameter, TResult> select)
			=> @this.Allow(I<TParameter>.Default).Or(select);

		public static ISource<T> Or<T>(this ISource<T> @this, ISource<T> fallback)
			=> @this.Or(IsAssigned<T>.Default, fallback);

		public static ISource<T> Or<T>(this ISource<T> @this, ISpecification<T> specification, ISource<T> fallback)
			=> new ValidatedSource<T>(specification, @this, fallback);

		public static ISelect<Unit, T> Empty<T>(this ISource<T> @this) => @this.Allow(I<Unit>.Default);

		public static ISelect<object, T> Any<T>(this ISource<T> @this) => @this.Allow(I<object>.Default);

		public static ISelect<TParameter, TResult> Allow<TParameter, TResult>(this ISource<TResult> @this, I<TParameter> _)
			=> new DelegatedResult<TParameter, TResult>(@this.Get);

		public static ISource<T> ToSource<T>(this T @this) => Sources<T>.Default.Get(@this);

		public static ISource<T> ToSource<T>(this Func<T> @this) => I<DelegatedSource<T>>.Default.From(@this);

		public static Func<T> ToDelegate<T>(this ISource<T> @this) => @this.Get;

		public static Func<T> ToDelegateReference<T>(this ISource<T> @this) => Model.Sources.Delegates<T>.Default.Get(@this);
	}
}
using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Runtime;
using Super.Runtime.Activation;
using System;
using System.Reactive;
using IAny = Super.Model.Specifications.IAny;

namespace Super
{
	public static partial class ExtensionMethods
	{
		public static ISource<T> AsSource<T>(this ISource<T> @this) => @this;

		/*public static ISelect<Unit, T> Empty<T>(this ISource<T> @this) => @this.Allow(I<Unit>.Default);*/

		public static ISource<T> Or<T>(this ISource<T> @this, ISource<T> fallback)
			=> @this.Or(IsAssigned<T>.Default, fallback);

		public static ISource<T> Or<T>(this ISource<T> @this, ISpecification<T> specification, ISource<T> fallback)
			=> new ValidatedSource<T>(specification, @this, fallback);

		/*public static ISource<TTo> Or<TFrom, TTo>(this ISource<TResult> @this,
		                                                                   ISelect<TParameter, TResult> select)
			=> IsAssigned<TResult>.Default.If();*/


		/*public static ISource<TTo> Or<TFrom, TTo>(this ISource<TFrom> @this, ISpecification<TFrom> specification,
		                                          ISelect<TFrom, TTo> select)
		=> */

		public static ISelect<TParameter, TResult> Allow<TParameter, TResult>(this ISource<TResult> @this, I<TParameter> _)
			=> new DelegatedResult<TParameter, TResult>(@this.Get);

		public static ISelect<object, T> Any<T>(this ISource<T> @this) => @this.Allow(I<object>.Default);

		public static IAny Exit<T>(this ISource<T> @this, ISpecification<T> specification)
			=> @this.Exit(specification.IsSatisfiedBy);

		public static IAny Exit<T>(this ISource<T> @this, ISelect<T, bool> specification)
			=> @this.Exit(specification.Get);

		public static IAny Exit<T>(this ISource<T> @this, Func<T, bool> specification)
			=> new DelegatedResultSpecification(new DelegatedSelection<T, bool>(specification, @this.ToDelegate()).Get).Any();

		public static Model.Commands.IAny Exit<T>(this ISource<T> @this, ISelect<T, Unit> select)
			=> @this.Exit(select.ToCommand());

		public static Model.Commands.IAny Exit<T>(this ISource<T> @this, ICommand<T> select)
			=> new DelegatedParameterCommand<T>(select.Execute, @this.Get).Any();

		public static ISelect<TFrom, TTo> Exit<TFrom, TTo>(this ISource<TTo> @this, ISelect<TFrom, TTo> select)
			=> @this.Exit(select.ToDelegate());

		public static ISelect<TFrom, TTo> Exit<TFrom, TTo>(this ISource<TTo> @this, Func<TFrom, TTo> select)
			=> @this.Exit(IsAssigned<TTo>.Default, @select);

		public static ISelect<TFrom, TTo> Exit<TFrom, TTo>(this ISource<TTo> @this, ISpecification<TTo> specification, Func<TFrom, TTo> select)
			=> new ValidatedResult<TFrom, TTo>(specification.IsSatisfiedBy, @this.Allow(I<TFrom>.Default).Get, select);

		public static ISource<TTo> Select<TFrom, TTo>(this ISource<TFrom> @this, I<TTo> _)
			=> @this.Select(Activations<TFrom, TTo>.Default);

		public static ISource<TTo> Select<TFrom, TTo>(this ISource<TFrom> @this, ISelect<TFrom, TTo> select)
			=> @this.Select(select.Get);

		public static ISource<TTo> Select<TFrom, TTo>(this ISource<TFrom> @this, Func<TFrom, TTo> select)
			=> new DelegatedSelection<TFrom,TTo>(select, @this.Get);

		public static ISource<T> ToSource<T>(this T @this) => Sources<T>.Default.Get(@this);

		public static ISource<T> ToSource<T>(this Func<T> @this) => I<DelegatedSource<T>>.Default.From(@this);

		public static Func<T> ToDelegate<T>(this ISource<T> @this) => @this.Get;

		public static Func<T> ToDelegateReference<T>(this ISource<T> @this) => Model.Sources.Delegates<T>.Default.Get(@this);
	}
}
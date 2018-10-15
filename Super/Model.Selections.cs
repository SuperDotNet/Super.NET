using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Runtime.Activation;
using Super.Runtime.Objects;
using System;
using System.Reactive;
using IAny = Super.Model.Specifications.IAny;

namespace Super
{
	// ReSharper disable once MismatchedFileName

	public static partial class ExtensionMethods
	{
		public static ISelect<object, TOut> Out<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> @this.Out(I<object>.Default);

		public static ISelect<TAllow, TOut> Out<TAllow, TIn, TOut>(this ISelect<TIn, TOut> @this, I<TAllow> allow)
			=> @this.ToDelegate().Out(allow);

		public static ISelect<TAllow, TOut> Out<TAllow, TIn, TOut>(this Func<TIn, TOut> @this, I<TAllow> allow)
			=> @this.Out(default)
			        .Out(allow)
			        .Unless(CanCast<TAllow, TIn>.Default, CastSelector<TAllow, TIn>.Default.Select(@this));

		/**/

		public static ISelect<_, TTo> Select<_, TFrom, TTo>(this ISelect<_, TFrom> @this, I<TTo> infer)
			where TTo : IActivateMarker<TFrom> => @this.Select(infer.From);

		public static ISelect<TParameter, TResult> Select<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISelect<Decoration<TParameter, TResult>, TResult> other)
			=> new Decorator<TParameter, TResult>(other, @this);

		public static ISelect<Unit, TOut> Select<TIn, TOut>(this ISelect<TIn, TOut> @this, TIn parameter)
			=> new FixedSelection<TIn, TOut>(@this, parameter).Out();

		public static ISelect<TIn, TTo> Select<TIn, TFrom, TTo>(this ISelect<TIn, TFrom> @this,
		                                                        ISource<ISelect<TFrom, TTo>> source)
			=> @this.Select(new DelegatedInstanceSelector<TFrom, TTo>(source).Get);

		public static ISelect<TIn, bool> Select<TIn, TFrom>(this ISelect<TIn, TFrom> @this,
		                                                    ISpecification<TFrom> select)
			=> @this.Select(select.IsSatisfiedBy);

		public static ISelect<TIn, TTo> Select<TIn, TFrom, TTo>(this ISelect<TIn, TFrom> @this,
		                                                        ISelect<TFrom, TTo> select)
			=> @this.Select(select.Get);

		public static ISelect<TIn, TTo> Select<TIn, TFrom, TTo>(this ISelect<TIn, TFrom> @this, Func<TFrom, TTo> select)
			=> new Selection<TIn, TFrom, TTo>(@this.Get, select);

		/**/

		public static ISelect<T, Unit> Out<T>(this ICommand<T> @this) => new Configuration<T>(@this.Execute);

		public static ISpecification<TIn> Out<TIn, TOut>(this ISelect<TIn, TOut> @this,
		                                                 ISelect<TOut, bool> specification)
			=> @this.Out(specification.Get);

		public static ISpecification<TIn> Out<TIn, TOut>(this ISelect<TIn, TOut> @this, Func<TOut, bool> specification)
			=> new SelectedParameterSpecification<TIn, TOut>(specification, @this.Get);

		public static ISpecification<TIn> Out<TIn, TOut>(this ISelect<TIn, TOut> @this,
		                                                 ISpecification<TOut> specification)
			=> @this.Out(specification.IsSatisfiedBy);

		public static ICommand<TIn> Out<TIn, TOut>(this ISelect<TIn, TOut> @this, ICommand<TOut> command)
			=> new SelectedParameterCommand<TIn, TOut>(command.Execute, @this.Get);

		public static ISpecification<T> Out<T>(this ISelect<T, bool> @this) => new DelegatedSpecification<T>(@this.Get);

		public static ICommand<T> Out<T>(this ISelect<T, Unit> @this) => new InvokeParameterCommand<T>(@this.Get);

		public static ISource<T> Out<T>(this ISelect<Unit, T> @this)
			=> new FixedSelection<Unit, T>(@this.Get, Unit.Default);

		public static ISelect<TIn, TOut> Out<TIn, TOut>(this Func<TIn, TOut> @this)
			=> Selections<TIn, TOut>.Default.Get(@this);

		public static IAny Out<TIn>(this ISelect<TIn, bool> @this, TIn parameter)
			=> new DelegatedResultSpecification(@this.Out<TIn, bool>(parameter).ToDelegate()).Any();

		public static Model.Commands.IAny Out<TIn>(this ISelect<TIn, Unit> @this, TIn parameter)
			=> new FixedParameterCommand<TIn>(@this.Out().Execute, parameter).Any();

		public static ISource<TOut> Out<TIn, TOut>(this ISelect<TIn, TOut> @this, TIn parameter)
			=> @this.ToDelegate().Out(parameter);

		public static ISource<TOut> Out<TIn, TOut>(this Func<TIn, TOut> @this, TIn parameter)
			=> new FixedSelection<TIn, TOut>(@this, parameter);

		public static ISource<TOut> Out<TIn, TOut>(this ISelect<TIn, TOut> @this, ISource<TIn> parameter)
			=> new DelegatedSelection<TIn, TOut>(@this, parameter);

		public static ISpecification<TFrom, TResult> Out<TFrom, TTo, TResult>(
			this ISelect<TFrom, TTo> @this, ISpecification<TTo, TResult> specification)
			=> new Specification<TFrom, TResult>(@this.Out(specification.IsSatisfiedBy),
			                                     @this.Select(specification.Get));
	}
}
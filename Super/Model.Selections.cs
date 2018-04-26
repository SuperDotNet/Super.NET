using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection;
using System;
using System.Reactive;
using IAny = Super.Model.Specifications.IAny;

// ReSharper disable TooManyArguments

namespace Super
{
	public static partial class ExtensionMethods
	{
		public static ISelect<TParameter, TResult> Select<TParameter, TResult>(this TResult @this, I<TParameter> _)
			=> new FixedResult<TParameter, TResult>(@this);
		public static ISelect<T, bool> Select<T>(this ISpecification<T> @this) => new Select<T, bool>(@this.IsSatisfiedBy);
		public static ISelect<T, Unit> Select<T>(this ICommand<T> @this) => new Configuration<T>(@this.Execute);
		public static ISelect<Unit, T> Select<T>(this ISource<T> @this) => new DelegatedResult<Unit, T>(@this.Get);

		public static IAny Enter<T>(this ISource<T> @this, ISpecification<T> specification)
			=> @this.Enter(specification.IsSatisfiedBy);

		public static IAny Enter<T>(this ISource<T> @this, ISelect<T, bool> specification)
			=> @this.Enter(specification.Get);

		public static IAny Enter<T>(this ISource<T> @this, Func<T, bool> specification)
			=> new DelegatedResultSpecification(new DelegatedSelection<T, bool>(specification, @this.ToDelegate()).Get).Any();

		public static Model.Commands.IAny Enter<T>(this ISource<T> @this, ISelect<T, Unit> select)
			=> @this.Enter(select.ToCommand());

		public static Model.Commands.IAny Enter<T>(this ISource<T> @this, ICommand<T> select)
			=> new DelegatedParameterCommand<T>(select.Execute, @this.Get).Any();

		public static ISelect<Unit, TOut> Enter<TIn, TOut>(this ISource<TIn> @this, ISelect<TIn, TOut> select)
			=> new DelegatedSelection<TIn, TOut>(select.Get, @this.Get).Select();

		public static ISpecification<TIn> Enter<TIn, TOut>(this ISelect<TIn, TOut> @this, ISpecification<TOut> specification)
			=> new SelectedParameterSpecification<TIn,TOut>(specification, @this);

		public static ICommand<TIn> Enter<TIn, TOut>(this ISelect<TIn, TOut> @this, ICommand<TOut> command)
			=> new SelectedParameterCommand<TIn, TOut>(command.Execute, @this.Get);

		public static ISpecification<T> Return<T>(this ISelect<T, bool> @this) => new DelegatedSpecification<T>(@this.Get);

		public static ICommand<T> Return<T>(this ISelect<T, Unit> @this) => new InvokeParameterCommand<T>(@this.Get);

		public static ISource<T> Return<T>(this ISelect<Unit, T> @this) => new FixedSelection<Unit, T>(@this.Get, Unit.Default);

		public static ISelect<TIn, TOut> Return<TIn, TOut>(this Func<TIn, TOut> @this) => Selections<TIn, TOut>.Default.Get(@this);

		public static IAny Fix<TIn>(this ISelect<TIn, bool> @this, TIn parameter)
			=> @this.Fix<TIn, bool>(parameter).ToSpecification().Any();

		public static Model.Commands.IAny Fix<TIn>(this ISelect<TIn, Unit> @this, TIn parameter)
			=> @this.Fix<TIn, Unit>(parameter).ToCommand().Any();

		public static ISelect<Unit, TOut> Fix<TIn, TOut>(this ISelect<TIn, TOut> @this, TIn parameter)
			=> new FixedSelection<TIn, TOut>(@this, parameter).Select();

		public static ISelect<TIn, TOut> If<TIn, TOut>(this ISelect<TIn, TOut> @this, ISpecification<TIn> specification)
			=> new Validated<TIn, TOut>(specification.IsSatisfiedBy, @this.Get);

		public static ISelect<TParameter, TResult> Into<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISelect<Decoration<TParameter, TResult>, TResult> other)
			=> new Decorator<TParameter, TResult>(other, @this);

		public static ISelect<TIn, TTo> Out<TIn, TFrom, TTo>(this ISelect<TIn, TFrom> @this, ISelect<TFrom, TTo> select)
			=> @this.Out(select.ToDelegate());

		public static ISelect<TIn, bool> Out<TIn, TFrom>(this ISelect<TIn, TFrom> @this, ISpecification<TFrom> specification)
			=> @this.Out(specification.ToDelegate());

		public static ISelect<TIn, TTo> Out<TIn, TFrom, TTo>(this ISelect<TIn, TFrom> @this, Func<TFrom, TTo> select)
			=> new Result<TIn,TFrom,TTo>(@this.Get, select);

		public static ISelect<TIn, TTo> Out<TIn, TFrom, TTo>(this ISelect<TIn, TFrom> @this, Func<ISelect<TIn, TFrom>, ISelect<TIn, TTo>> configure)
			=> configure(new Select<TIn, TFrom>(@this.Get));

		public static ISource<TTo> Out<TFrom, TTo>(this ISource<TFrom> @this, ISelect<TFrom, TTo> select)
			=> @this.Out(select.ToDelegate());

		public static ISource<TTo> Out<TFrom, TTo>(this ISource<TFrom> @this, Func<TFrom, TTo> select)
			=> @this.Select().Out(select).Return();

		public static ISource<TTo> Out<TFrom, TTo>(this ISource<TFrom> @this,
		                                           Func<ISelect<Unit, TFrom>, ISelect<Unit, TTo>> configure)
			=> configure(@this.Select()).Return();

		public static ISpecification<TFrom, TResult> Out<TFrom, TTo, TResult>(this ISelect<TFrom, TTo> @this,
		                                                                      ISpecification<TTo, TResult> specification)
			=> @this.Out(specification.AsSelect()).ToSpecification(@this.Out(specification.IsSatisfiedBy).ToSpecification());
	}
}
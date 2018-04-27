﻿using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Reflection.Types;
using Super.Runtime.Activation;
using System;
using System.Reactive;
using IAny = Super.Model.Specifications.IAny;

// ReSharper disable TooManyArguments

namespace Super
{
	public static partial class ExtensionMethods
	{
		public static ISelect<TParameter, TResult> Enter<TParameter, TResult>(this TResult @this, I<TParameter> _)
			=> new FixedResult<TParameter, TResult>(@this);

		public static ISelect<T, bool> Enter<T>(this ISpecification<T> @this) => new Select<T, bool>(@this.IsSatisfiedBy);

		public static ISelect<T, Unit> Enter<T>(this ICommand<T> @this) => new Configuration<T>(@this.Execute);

		public static ISelect<Unit, T> Enter<T>(this ISource<T> @this) => new DelegatedResult<Unit, T>(@this.Get);

		public static ISpecification<TTo> Select<TFrom, TTo>(this ISpecification<TFrom> @this,
		                                                     Func<ISelect<TFrom, bool>, ISelect<TTo, bool>> configure)
			=> configure(@this.Enter()).Exit();

		public static ICommand<TTo> Select<TFrom, TTo>(this ICommand<TFrom> @this,
		                                               Func<ISelect<TFrom, Unit>, ISelect<TTo, Unit>> configure)
			=> configure(@this.Enter()).Exit();

		public static ISource<TTo> Select<TFrom, TTo>(this ISource<TFrom> @this,
		                                              Func<ISelect<Unit, TFrom>, ISelect<Unit, TTo>> configure)
			=> configure(@this.Enter()).Exit();

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

		public static ISource<TOut> Exit<TIn, TOut>(this ISource<TIn> @this, I<TOut> _) => @this.Exit(Activations<TIn, TOut>.Default);

		public static ISource<TOut> Exit<TIn, TOut>(this ISource<TIn> @this, ISelect<TIn, TOut> select) => @this.Exit(select.ToDelegate());

		public static ISource<TOut> Exit<TIn, TOut>(this ISource<TIn> @this, Func<TIn, TOut> select)
			=> new DelegatedSelection<TIn, TOut>(select, @this.Get);

		public static ISpecification<TIn> Exit<TIn, TOut>(this ISelect<TIn, TOut> @this, ISpecification<TOut> specification)
			=> new SelectedParameterSpecification<TIn, TOut>(specification, @this);

		public static ICommand<TIn> Exit<TIn, TOut>(this ISelect<TIn, TOut> @this, ICommand<TOut> command)
			=> new SelectedParameterCommand<TIn, TOut>(command.Execute, @this.Get);

		/*public static ISource<T> Exit<T>(this ISource<Type> @this, IGeneric<T> generic)
			=> @this.Enter().Sequence().Enumerate().Out(generic).Invoke().Exit();*/

		/*public static ISelect<TIn, TResult> Exit<TIn, TOut, TResult>(this ISelect<TIn, TOut> @this, IGeneric<TResult> generic)
			=> @this.Type().Exit(generic);*/

		public static ISelect<TIn, TResult> Exit<TIn, TResult>(this ISelect<TIn, Type> @this, IGeneric<TResult> generic)
			=> @this.Sequence().Enumerate().Out(generic).Invoke();

		public static ISpecification<T> Exit<T>(this ISelect<T, bool> @this) => new DelegatedSpecification<T>(@this.Get);

		public static ICommand<T> Exit<T>(this ISelect<T, Unit> @this) => new InvokeParameterCommand<T>(@this.Get);

		public static ISource<T> Exit<T>(this ISelect<Unit, T> @this) => new FixedSelection<Unit, T>(@this.Get, Unit.Default);

		public static ISelect<TIn, TOut> Exit<TIn, TOut>(this Func<TIn, TOut> @this)
			=> Selections<TIn, TOut>.Default.Get(@this);

		public static IAny Exit<TIn>(this ISelect<TIn, bool> @this, TIn parameter)
			=> new DelegatedResultSpecification(@this.Exit<TIn, bool>(parameter).ToDelegate()).Any();

		public static Model.Commands.IAny Exit<TIn>(this ISelect<TIn, Unit> @this, TIn parameter)
			=> new FixedParameterCommand<TIn>(@this.ToCommand().Execute, parameter).Any();

		public static ISource<TOut> Exit<TIn, TOut>(this ISelect<TIn, TOut> @this, TIn parameter)
			=> new FixedSelection<TIn, TOut>(@this, parameter);

		public static ISelect<Unit, TOut> Close<TIn, TOut>(this ISelect<TIn, TOut> @this, TIn parameter)
			=> new FixedSelection<TIn, TOut>(@this, parameter).Enter();

		public static ISelect<TIn, TOut> If<TIn, TOut>(this ISelect<TIn, TOut> @this, ISpecification<TIn> specification)
			=> new Validated<TIn, TOut>(specification.IsSatisfiedBy, @this.Get);

		public static ISelect<TParameter, TResult> Into<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISelect<Decoration<TParameter, TResult>, TResult> other)
			=> new Decorator<TParameter, TResult>(other, @this);

		public static ISelect<TIn, bool> Out<TIn, TFrom>(this ISelect<TIn, TFrom> @this, ISpecification<TFrom> specification)
			=> @this.Out(specification.ToDelegate());

		public static ISelect<TIn, TTo> Out<TIn, TFrom, TTo>(this ISelect<TIn, TFrom> @this, ISource<ISelect<TFrom, TTo>> source)
			=> @this.Out(new DelegatedInstanceSelector<TFrom, TTo>(source).Get);

		public static ISelect<TIn, TTo> Out<TIn, TFrom, TTo>(this ISelect<TIn, TFrom> @this, ISelect<TFrom, TTo> select)
			=> @this.Out(select.ToDelegate());

		public static ISelect<TIn, TTo> Out<TIn, TFrom, TTo>(this ISelect<TIn, TFrom> @this, Func<TFrom, TTo> select)
			=> new Result<TIn, TFrom, TTo>(@this.Get, select);

		public static ISelect<TIn, TTo> Out<TIn, TFrom, TTo>(this ISelect<TIn, TFrom> @this,
		                                                     Func<ISelect<TIn, TFrom>, ISelect<TIn, TTo>> configure)
			=> configure(new Select<TIn, TFrom>(@this.Get));

		/*public static ISource<TTo> Out<TFrom, TTo>(this ISource<TFrom> @this, ISelect<TFrom, TTo> select)
			=> @this.Out(select.ToDelegate());

		public static ISource<TTo> Out<TFrom, TTo>(this ISource<TFrom> @this, Func<TFrom, TTo> select)
			=> @this.Enter().Out(select).Exit();*/

		/*public static ISource<TTo> Out<TFrom, TTo>(this ISource<TFrom> @this,
		                                           Func<ISelect<Unit, TFrom>, ISelect<Unit, TTo>> configure)
			=> configure(@this.Enter()).Exit();*/

		public static ISpecification<TFrom, TResult> Out<TFrom, TTo, TResult>(this ISelect<TFrom, TTo> @this,
		                                                                      ISpecification<TTo, TResult> specification)
			=> @this.Out(specification.AsSelect()).ToSpecification(@this.Out(specification.IsSatisfiedBy).ToSpecification());
	}
}
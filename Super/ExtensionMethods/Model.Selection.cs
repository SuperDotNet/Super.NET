using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Selection.Stores;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Runtime;
using Super.Text;
using System;
using System.Collections.Immutable;
using System.Reactive;

namespace Super.ExtensionMethods
{
	partial class Model
	{
		public static ISelect<TParameter, TResult> Guard<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this) => @this.Or(GuardedFallback<TParameter, TResult>.Default);

		public static ISelect<TParameter, TResult> Guard<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, Func<TParameter, string> message)
			=> @this.Guard(new Message<TParameter>(message));

		public static ISelect<TParameter, TResult> Guard<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, IMessage<TParameter> message)
			=> @this.Or(new GuardedFallback<TParameter, TResult>(message));

		public static ISelect<TParameter, TResult> Try<TException, TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, I<TException> infer) where TException : Exception
			=> infer.Try(@this.ToDelegate(), @this.Default().ToDelegate());

		public static ISelect<TParameter, TResult> Default<TParameter, TResult>(this ISelect<TParameter, TResult> @this)
			=> Defaults<TParameter, TResult>.Default.Get(@this);

		public static ISelect<TParameter, TResult> Or<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISource<TResult> source)
			=> @this.Or(source.Allow(I<TParameter>.Default));

		/*public static ISource<TParameter, TResult> Or<TParameter, TResult>(
			this ISource<TParameter, TResult> @this) => @this.Or(@this.Default());*/

		public static ISelect<TParameter, TResult> Or<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISelect<TParameter, TResult> next)
			=> @this.Or(IsAssigned<TResult>.Default, next);

		public static ISelect<TParameter, TResult> Or<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISpecification<TResult> specification)
			=> @this.Or(specification, @this.Default());

		public static ISelect<TParameter, TResult> Or<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISpecification<TResult> specification,
			ISelect<TParameter, TResult> fallback)
			=> @this.ToDelegate().Or(specification.ToDelegate(), fallback.ToDelegate());

		public static ISelect<TParameter, TResult> Or<TParameter, TResult>(
			this Func<TParameter, TResult> @this, Func<TResult, bool> specification,
			Func<TParameter, TResult> fallback)
			=> new ValidatedResult<TParameter, TResult>(specification, @this, fallback);

		public static ISelect<TParameter, TResult> Unless<TParameter, TResult, TOther>(
			this ISelect<TParameter, TResult> @this, ISelect<TOther, TResult> other)
			=> IsTypeSpecification<TParameter, TOther>.Default.If(other.In(Cast<TParameter>.Default), @this);

		public static ISelect<TParameter, TResult> Configure<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, IAssignable<TParameter, TResult> assignable)
			=> new Configuration<TParameter, TResult>(@this, assignable);

		public static ISelect<TParameter, TResult> Assigned<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this) => IsAssigned<TParameter>.Default.If(@this);

		public static T Get<T>(this ISelect<Unit, T> @this) => @this.Get(Unit.Default);

		public static Func<TParameter, TResult> ToDelegate<TParameter, TResult>(this ISelect<TParameter, TResult> @this)
			=> Delegates<TParameter, TResult>.Default.Get(@this);

		public static ISelect<TParameter, TResult> ToStore<TParameter, TResult>(this ISelect<TParameter, TResult> @this)
			where TParameter : class => @this.ToDelegate().ToStore();

		public static ISpecification<TParameter, TResult> ToStore<TParameter, TResult>(
			this ISpecification<TParameter, TResult> @this)
			where TParameter : class
			=> new Specification<TParameter, TResult>(ToDelegate((ISpecification<TParameter>)@this).ToStore().ToDelegate(),
			                                          ToDelegate((ISelect<TParameter, TResult>)@this).ToStore().ToDelegate());

		public static ISelect<TParameter, TResult> ToStore<TParameter, TResult>(this Func<TParameter, TResult> @this)
			where TParameter : class => ReferenceStores<TParameter, TResult>.Default.Get(@this);

		public static ISelect<TParameter, TResult> ToSource<TParameter, TResult>(
			this TResult @this, ISpecification<TParameter> specification)
			=> new Conditional<TParameter, TResult>(specification, @this, default);

		public static ISelect<Unit, T> ToSource<T>(this T @this)
			=> @this.ToSource(I<Unit>.Default);

		public static ISelect<TParameter, TResult> ToSource<TParameter, TResult>(this TResult @this, I<TParameter> _)
			=> new FixedResult<TParameter, TResult>(@this);

		public static ISelect<TParameter, TResult> ToSource<TParameter, TResult>(this Func<TParameter, TResult> @this)
			=> Selections<TParameter, TResult>.Default.Get(@this);

		public static ISelect<TParameter, TResult> ToSource<TParameter, TResult, TAttribute>(
			this TResult @this, I<TAttribute> _) where TAttribute : Attribute
			=> @this.ToSource(IsDefinedSpecification<TAttribute>
			                  .Default
			                  .Select(InstanceMetadataSelector<TParameter>.Default));

		public static TResult Get<TItem, TResult>(this ISelect<ImmutableArray<TItem>, TResult> @this,
		                                          params TItem[] parameters)
			=> @this.Get(parameters.ToImmutableArray());
	}
}
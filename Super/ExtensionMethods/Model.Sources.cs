using Super.Model.Containers;
using Super.Model.Instances;
using Super.Model.Sources;
using Super.Model.Sources.Coercion;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Runtime;
using System;
using System.Collections.Immutable;
using System.Reactive;

namespace Super.ExtensionMethods
{
	partial class Model
	{
		public static ISource<TParameter, TResult> Guard<TParameter, TResult>(
			this ISource<TParameter, TResult> @this) => @this.Or(GuardedFallback<TParameter, TResult>.Default);

		public static ISource<TParameter, TResult> Guard<TParameter, TResult>(
			this ISource<TParameter, TResult> @this, Func<TParameter, string> message)
			=> @this.Guard(new Message<TParameter>(message));

		public static ISource<TParameter, TResult> Guard<TParameter, TResult>(
			this ISource<TParameter, TResult> @this, IMessage<TParameter> message)
			=> @this.Or(new GuardedFallback<TParameter, TResult>(message));

		public static ISource<TParameter, TResult> Try<TException, TParameter, TResult>(
			this ISource<TParameter, TResult> @this, I<TException> infer) where TException : Exception
			=> infer.Try(@this.ToDelegate(), @this.Default().ToDelegate());

		public static ISource<TParameter, TResult> Default<TParameter, TResult>(this ISource<TParameter, TResult> @this)
			=> Defaults<TParameter, TResult>.Default.Get(@this);

		public static ISource<TParameter, TResult> Or<TParameter, TResult>(
			this ISource<TParameter, TResult> @this, IInstance<TResult> instance)
			=> @this.Or(instance.Allow(I<TParameter>.Default));

		/*public static ISource<TParameter, TResult> Or<TParameter, TResult>(
			this ISource<TParameter, TResult> @this) => @this.Or(@this.Default());*/

		public static ISource<TParameter, TResult> Or<TParameter, TResult>(
			this ISource<TParameter, TResult> @this, ISource<TParameter, TResult> next)
			=> @this.Or(IsAssigned<TResult>.Default, next);

		public static ISource<TParameter, TResult> Or<TParameter, TResult>(
			this ISource<TParameter, TResult> @this, ISpecification<TResult> specification) => @this.Or(specification, @this.Default());

		public static ISource<TParameter, TResult> Or<TParameter, TResult>(
			this ISource<TParameter, TResult> @this, ISpecification<TResult> specification,
			ISource<TParameter, TResult> fallback)
			=> @this.ToDelegate().Or(specification.ToDelegate(), fallback.ToDelegate());

		public static ISource<TParameter, TResult> Or<TParameter, TResult>(
			this Func<TParameter, TResult> @this, Func<TResult, bool> specification,
			Func<TParameter, TResult> fallback)
			=> new ValidatedResult<TParameter, TResult>(specification, @this, fallback);

		public static ISource<TParameter, TResult> Unless<TParameter, TResult, TOther>(
			this ISource<TParameter, TResult> @this, ISource<TOther, TResult> other)
			=> IsTypeSpecification<TParameter, TOther>.Default.If(other.In(Cast<TParameter>.Default), @this);

		public static ISource<TParameter, TResult> Configure<TParameter, TResult>(
			this ISource<TParameter, TResult> @this, IAssignable<TParameter, TResult> assignable)
			=> new ConfiguringSource<TParameter, TResult>(@this, assignable);

		public static ISource<TParameter, TResult> Assigned<TParameter, TResult>(
			this ISource<TParameter, TResult> @this) => IsAssigned<TParameter>.Default.If(@this);

		public static T Get<T>(this ISource<Unit, T> @this) => @this.Get(Unit.Default);

		public static Func<TParameter, TResult> ToDelegate<TParameter, TResult>(this ISource<TParameter, TResult> @this)
			=> Delegates<TParameter, TResult>.Default.Get(@this);

		public static ISource<TParameter, TResult> ToStore<TParameter, TResult>(this ISource<TParameter, TResult> @this)
			where TParameter : class => @this.ToDelegate().ToStore();

		public static ISpecification<TParameter, TResult> ToStore<TParameter, TResult>(
			this ISpecification<TParameter, TResult> @this)
			where TParameter : class
			=> new SpecificationSource<TParameter, TResult>(Specifications.ToDelegate(@this).ToStore().ToDelegate(),
			                                                ToDelegate(@this).ToStore().ToDelegate());

		public static ISource<TParameter, TResult> ToStore<TParameter, TResult>(this Func<TParameter, TResult> @this)
			where TParameter : class => ReferenceStores<TParameter, TResult>.Default.Get(@this);

		public static ISource<TParameter, TResult> ToSource<TParameter, TResult>(
			this TResult @this, ISpecification<TParameter> specification)
			=> new ConditionalInstance<TParameter, TResult>(specification, @this, default);

		public static ISource<Unit, T> ToSource<T>(this T @this)
			=> @this.ToSource(I<Unit>.Default);

		public static ISource<TParameter, TResult> ToSource<TParameter, TResult>(this TResult @this, I<TParameter> _)
			=> new FixedResult<TParameter, TResult>(@this);

		public static ISource<TParameter, TResult> ToSource<TParameter, TResult>(this Func<TParameter, TResult> @this)
			=> Sources<TParameter, TResult>.Default.Get(@this);

		public static ISource<TParameter, TResult> ToSource<TParameter, TResult, TAttribute>(
			this TResult @this, I<TAttribute> _) where TAttribute : Attribute
			=> @this.ToSource(IsDefinedSpecification<TAttribute>
			                  .Default
			                  .Select(InstanceMetadataCoercer<TParameter>.Default));

		public static TResult Get<TItem, TResult>(this ISource<ImmutableArray<TItem>, TResult> @this,
		                                          params TItem[] parameters)
			=> @this.Get(parameters.ToImmutableArray());
	}
}
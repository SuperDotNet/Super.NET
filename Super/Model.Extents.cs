using Super.Model.Collections;
using Super.Model.Extents;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Reflection.Types;
using Super.Runtime.Activation;
using Super.Runtime.Objects;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reactive;
using System.Reflection;

namespace Super
{
	public static partial class ExtensionMethods
	{
		public static IIn<Unit, TOut> In<TIn, TOut>(this IIn<TIn, TOut> @this, ISource<TIn> parameter)
			=> @this.In(parameter.Get);

		public static IIn<Unit, TOut> In<TIn, TOut>(this IIn<TIn, TOut> @this, Func<TIn> parameter)
			=> new DelegatedSelection<TIn, TOut>(@this.Get(), parameter).Empty().In();

		public static IIn<Unit, TOut> In<TIn, TOut>(this IIn<TIn, TOut> @this, TIn parameter)
			=> new FixedSelection<TIn, TOut>(@this.Get(), parameter).Empty().In();

		public static IIn<TFrom, TOut> In<TFrom, TTo, TOut>(this IIn<TTo, TOut> @this, ISelect<TFrom, TTo> select)
			=> @this.In(select.ToDelegate());

		public static IIn<TIn, TOut> In<TIn, TOut>(this IIn<TIn, TOut> @this, ISpecification<TIn> specification)
			=> new Validated<TIn, TOut>(specification.IsSatisfiedBy, @this.Get()).In();

		public static IIn<TFrom, TOut> In<TFrom, TTo, TOut>(this IIo<TTo, TOut> @this, Func<TFrom, TTo> select)
			=> new In<TFrom, TOut>(new Parameter<TFrom, TTo, TOut>(@this.Get(), select).Get);

		public static IOut<TIn, TTo> Out<TIn, TFrom, TTo>(this IIo<TIn, TFrom> @this, ISelect<TFrom, TTo> select)
			=> @this.Out(select.ToDelegate());

		public static IOut<TIn, TTo> Out<TIn, TFrom, TTo>(this IIo<TIn, TFrom> @this, Func<TFrom, TTo> select)
			=> new Out<TIn, TTo>(new Result<TIn, TFrom, TTo>(@this.Get(), select).Get);

		public static IIn<TTo, TOut> Cast<TIn, TOut, TTo>(this IIn<TIn, TOut> @this, I<TTo> _)
			=> @this.In(Cast<TTo, TIn>.Default);

		public static IOut<TIn, TTo> Cast<TIn, TOut, TTo>(this IIo<TIn, TOut> @this, I<TTo> _)
			=> @this.Out(Cast<TOut, TTo>.Default);

		public static IIn<TNew, TOut> New<TIn, TOut, TNew>(this IIn<TIn, TOut> @this, I<TNew> _)
			=> @this.In(Activations<TNew, TIn>.Default.ToDelegate());

		public static IOut<TIn, TNew> New<TIn, TOut, TNew>(this IOut<TIn, TOut> @this, I<TNew> _)
			=> @this.Out(Activations<TOut, TNew>.Default.ToDelegate());

		public static IOut<TIn, TNew> Activate<TIn, TOut, TNew>(this IOut<TIn, TOut> @this, I<TNew> _) where TNew : IActivateMarker<TOut>
			=> @this.Out(MarkedActivations<TOut, TNew>.Default.ToDelegate());

		public static IOut<TIn, TAttribute> Attribute<TIn, TAttribute>(this IOut<TIn, ICustomAttributeProvider> @this, I<TAttribute> _)
			=> @this.Out(Attribute<TAttribute>.Default.ToDelegate());

		public static IOut<TIn, TTo> Fold<TIn, TFrom, TTo>(this IOut<TIn, ISelect<TFrom, TTo>> @this) => @this.Fold(Activation<TFrom>.Default);

		public static IOut<TIn, TTo> Fold<TIn, TFrom, TTo>(this IOut<TIn, ISelect<TFrom, TTo>> @this, ISource<TFrom> select)
			=> @this.Out(select.Out).Value();

		public static IIn<TIn, TOut> Type<TIn, TOut>(this IIn<Type, TOut> @this, I<TIn> _)
			=> @this.In(InstanceTypeSelector<TIn>.Default);

		public static IOut<TIn, Type> Type<TIn, TOut>(this IOut<TIn, TOut> @this)
			=> @this.Out(InstanceTypeSelector<TOut>.Default);

		public static IIn<Type, T> Metadata<T>(this IIn<TypeInfo, T> @this) => @this.In(TypeMetadataSelector.Default);

		public static IOut<T, TypeInfo> Metadata<T>(this IOut<T, Type> @this) => @this.Out(TypeMetadataSelector.Default);

		public static IIn<TIn, TOut> Once<TIn, TOut>(this IIn<TIn, TOut> @this)
			=> OnceAlteration<TIn, TOut>.Default.Get(@this);

		public static IIn<TIn, TOut> OnlyOnce<TIn, TOut>(this IIn<TIn, TOut> @this)
			=> OnlyOnceAlteration<TIn, TOut>.Default.Get(@this);

		public static IIn<T, bool> Once<T>(this IIn<T, bool> @this) => OnceAlteration<T>.Default.Get(@this);

		public static IIn<T, bool> OnlyOnce<T>(this IIn<T, bool> @this) => OnlyOnceAlteration<T>.Default.Get(@this);

		public static IIn<T, bool> And<T>(this IIn<T, bool> @this, params IIn<T, bool>[] others)
			=> new AllSpecification<T>(others.Prepend(@this).Select(x => x.Return()).Fixed()).In();

		public static IOut<T, bool> And<T>(this IOut<T, bool> @this, params IIo<T, bool>[] others)
			=> new Out<T, bool>(new AllSpecification<T>(others.Prepend(@this).Select(x => x.Get()).ToImmutableArray()).IsSatisfiedBy);

		public static IIn<T, bool> Or<T>(this IIn<T, bool> @this, params IIn<T, bool>[] others)
			=> new AnySpecification<T>(others.Prepend(@this).Select(x => x.Return()).Fixed()).In();

		public static IOut<T, bool> Or<T>(this IOut<T, bool> @this, params IOut<T, bool>[] others)
			=> new Out<T, bool>(new AnySpecification<T>(others.Prepend(@this).Select(x => x.Return()).Fixed()).IsSatisfiedBy);


		public static IIn<object, TOut> Allow<TOut>(this IIn<Unit, TOut> @this) => @this.Allow(I<object>.Default);

		public static IIn<T, TOut> Allow<T, TOut>(this IIn<Unit, TOut> @this, I<T> _)
			=> @this.In<T, Unit, TOut>(__ => Unit.Default);

		public static IOut<TIn, bool> HasAny<TIn, TOut>(this IOut<TIn, ICollection<TOut>> @this)
			=> @this.Out(HasAny<TOut>.Default.IsSatisfiedBy);

		public static IOut<TIn, bool> HasNone<TIn, TOut>(this IOut<TIn, ICollection<TOut>> @this)
			=> @this.Out(HasNone<TOut>.Default.IsSatisfiedBy);

		public static IOut<TIn, bool> AllAre<TIn, TOut>(this IOut<TIn, ICollection<TOut>> @this, Func<TOut, bool> condition)
			=> @this.Out(new AllItemsAre<TOut>(condition).IsSatisfiedBy);

		public static IOut<TIn, TOut> Value<TIn, TOut>(this IOut<TIn, ISource<TOut>> @this) => @this.Out(ValueSelector<TOut>.Default);
	}
}
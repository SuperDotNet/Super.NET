using Super.Model.Collections;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Reflection.Types;
using Super.Runtime.Activation;
using Super.Runtime.Invocation;
using Super.Runtime.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reflection;

namespace Super
{
	public static partial class ExtensionMethods
	{
		public static ISelect<TIn, TTo> Cast<TIn, TOut, TTo>(this ISelect<TIn, TOut> @this, I<TTo> _)
			=> @this.Out(Cast<TOut, TTo>.Default);

		public static ISelect<TIn, TTo> CastForValue<TIn, TOut, TTo>(this ISelect<TIn, TOut> @this, I<TTo> _)
			=> @this.Out(ValueAwareCast<TOut, TTo>.Default);

		public static ISelect<TIn, TNew> New<TIn, TOut, TNew>(this ISelect<TIn, TOut> @this, I<TNew> _)
			=> @this.Out(Activations<TOut, TNew>.Default.ToDelegate());

		public static ISelect<TIn, TNew> Activate<TIn, TOut, TNew>(this ISelect<TIn, TOut> @this, I<TNew> _)
			where TNew : IActivateMarker<TOut>
			=> @this.Out(MarkedActivations<TOut, TNew>.Default.ToDelegate());

		public static ISelect<TIn, TAttribute> Attribute<TIn, TAttribute>(this ISelect<TIn, ICustomAttributeProvider> @this,
		                                                                  I<TAttribute> _)
			=> @this.Out(Attribute<TAttribute>.Default.ToDelegate());

		public static ISelect<TIn, TTo> Fold<TIn, TFrom, TTo>(this ISelect<TIn, ISelect<TFrom, TTo>> @this)
			=> @this.Fold(Activation<TFrom>.Default);

		public static ISelect<TIn, TTo> Fold<TIn, TFrom, TTo>(this ISelect<TIn, ISelect<TFrom, TTo>> @this,
		                                                      ISource<TFrom> select)
			=> @this.Out(select.Out).Value();

		public static ISelect<TIn, Type> Type<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> @this.Out(InstanceTypeSelector<TOut>.Default);

		public static ISelect<T, TypeInfo> Metadata<T>(this ISelect<T, Type> @this)
			=> @this.Out(TypeMetadataSelector.Default);

		public static ISelect<TIn, TOut> OnceStriped<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> OncePerParameter<TIn, TOut>.Default.Get(@this);

		public static ISelect<T, bool> OnceStriped<T>(this ISelect<T, bool> @this)
			=> OncePerParameter<T, bool>.Default.Get(@this).And(@this);

		public static ISelect<TIn, TOut> OnlyOnce<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> OnlyOnceAlteration<TIn, TOut>.Default.Get(@this);

		public static ISelect<T, bool> OnlyOnce<T>(this ISelect<T, bool> @this)
			=> OnlyOnceAlteration<T, bool>.Default.Get(@this).And(@this);

		public static ISelect<T, bool> And<T>(this ISelect<T, bool> @this, params ISelect<T, bool>[] others)
			=> @this.And(others.Select(x => x.ToDelegate()).Fixed());

		public static ISelect<T, bool> And<T>(this ISelect<T, bool> @this, params Func<T, bool>[] others)
			=> new AllSpecification<T>(others.Prepend(@this.Get).Fixed()).Start();

		public static ISelect<T, bool> Or<T>(this ISelect<T, bool> @this, params ISelect<T, bool>[] others)
			=> @this.Or(others.Select(x => x.ToDelegate()).Fixed());

		public static ISelect<T, bool> Or<T>(this ISelect<T, bool> @this, params Func<T, bool>[] others)
			=> new AnySpecification<T>(others.Prepend(@this.Get).Fixed()).Start();

		public static ISelect<T, TOut> Allow<T, TOut>(this ISelect<Unit, TOut> @this, I<T> _)
			=> In<T>.Select(__ => Unit.Default).Out(@this);

		public static ISelect<TIn, bool> HasAny<TIn, TOut>(this ISelect<TIn, ICollection<TOut>> @this)
			=> @this.Out(HasAny<TOut>.Default.IsSatisfiedBy);

		public static ISelect<TIn, bool> HasNone<TIn, TOut>(this ISelect<TIn, ICollection<TOut>> @this)
			=> @this.Out(HasNone<TOut>.Default.IsSatisfiedBy);

		public static ISelect<TIn, bool> AllAre<TIn, TOut>(this ISelect<TIn, ICollection<TOut>> @this,
		                                                   Func<TOut, bool> condition)
			=> @this.Out(new AllItemsAre<TOut>(condition).IsSatisfiedBy);

		public static ISelect<TIn, TOut> Value<TIn, TOut>(this ISelect<TIn, ISource<TOut>> @this)
			=> @this.Out(ValueSelector<TOut>.Default);

		public static ISelect<TIn, TOut> Invoke<TIn, TOut>(this ISelect<TIn, Func<TOut>> @this)
			=> @this.Out(Call<TOut>.Default);
	}
}
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
using System.Reflection;

namespace Super
{
	public static partial class ExtensionMethods
	{
		public static ISelect<TIn, TTo> Cast<TIn, TOut, TTo>(this ISelect<TIn, TOut> @this, I<TTo> _)
			=> @this.Select(Cast<TOut, TTo>.Default);

		public static ISelect<TIn, TOut> As<TIn, TOut, TTo>(this ISelect<TIn, TOut> @this, I<TTo> _,
		                                                    Func<ISelect<TIn, TTo>, ISelect<TIn, TTo>> select)
			=> select(@this.Cast(I<TTo>.Default)).Assigned().Cast(I<TOut>.Default);

		public static ISelect<TIn, TTo> CastForValue<TIn, TOut, TTo>(this ISelect<TIn, TOut> @this, I<TTo> _)
			=> @this.Select(ValueAwareCast<TOut, TTo>.Default);

		public static ISelect<TIn, TNew> New<TIn, TOut, TNew>(this ISelect<TIn, TOut> @this, I<TNew> _)
			=> @this.Select(Activations<TOut, TNew>.Default.ToDelegate());

		public static ISelect<TIn, TNew> Activate<TIn, TOut, TNew>(this ISelect<TIn, TOut> @this, I<TNew> _)
			where TNew : IActivateMarker<TOut>
			=> @this.Select(MarkedActivations<TOut, TNew>.Default.ToDelegate());

		public static ISelect<TIn, TAttribute> Attribute<TIn, TAttribute>(this ISelect<TIn, ICustomAttributeProvider> @this,
		                                                                  I<TAttribute> _)
			=> @this.Select(Attribute<TAttribute>.Default.ToDelegate());

		public static ISelect<TIn, TTo> Fold<TIn, TFrom, TTo>(this ISelect<TIn, ISelect<TFrom, TTo>> @this)
			=> @this.Fold(New<TFrom>.Default);

		public static ISelect<TIn, TTo> Fold<TIn, TFrom, TTo>(this ISelect<TIn, ISelect<TFrom, TTo>> @this,
		                                                      ISource<TFrom> select)
			=> @this.Select(select.Select).Value();

		public static ISelect<TIn, Type> Type<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> @this.Select(InstanceTypeSelector<TOut>.Default);

		public static ISelect<T, TypeInfo> Metadata<T>(this ISelect<T, Type> @this)
			=> @this.Select(TypeMetadataSelector.Default);

		/*public static ISelect<T, Type> Type<T>(this ISelect<T, TypeInfo> @this) => @this.Out(TypeSelector.Default);*/

		public static ISelect<TIn, TOut> OnceStriped<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> OncePerParameter<TIn, TOut>.Default.Get(@this);

		/*public static ISelect<T, bool> OnceStriped<T>(this ISelect<T, bool> @this)
			=> OncePerParameter<T, bool>.Default.Get(@this).And(@this);*/

		public static ISelect<TIn, TOut> OnlyOnce<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> OnlyOnceAlteration<TIn, TOut>.Default.Get(@this);

		/*public static ISelect<T, bool> OnlyOnce<T>(this ISelect<T, bool> @this)
			=> OnlyOnceAlteration<T, bool>.Default.Get(@this).And(@this);*/

		/*public static ISelect<T, bool> And<T>(this ISelect<T, bool> @this, params ISelect<T, bool>[] others)
			=> @this.And(others.Select(x => x.ToDelegate()).Fixed());

		public static ISelect<T, bool> And<T>(this ISelect<T, bool> @this, params Func<T, bool>[] others)
			=> new AllSpecification<T>(others.Prepend(@this.Get).Fixed()).Enter();*/

		/*public static ISelect<T, bool> Or<T>(this ISelect<T, bool> @this, params ISelect<T, bool>[] others)
			=> @this.Or(others.Select(x => x.ToDelegate()).Fixed());

		public static ISelect<T, bool> Or<T>(this ISelect<T, bool> @this, params Func<T, bool>[] others)
			=> new AnySpecification<T>(others.Prepend(@this.Get).Fixed()).Enter();*/

		public static ISpecification<TIn> HasAny<TIn, TOut>(this ISelect<TIn, ICollection<TOut>> @this)
			=> @this.Out(HasAny<TOut>.Default);

		public static ISpecification<TIn> HasNone<TIn, TOut>(this ISelect<TIn, ICollection<TOut>> @this)
			=> @this.Out(HasNone<TOut>.Default);

		public static ISpecification<TIn> AllAre<TIn, TOut>(this ISelect<TIn, ICollection<TOut>> @this,
		                                                    Func<TOut, bool> condition)
			=> @this.Out(new AllItemsAre<TOut>(condition));

		public static ISelect<TIn, TOut> Value<TIn, TOut>(this ISelect<TIn, ISource<TOut>> @this)
			=> @this.Select(ValueSelector<TOut>.Default);

		public static ISelect<TIn, TOut> Invoke<TIn, TOut>(this ISelect<TIn, Func<TOut>> @this)
			=> @this.Select(Call<TOut>.Default);

		/*public static ISelect<TIn, Func<TParameter, TResult>> Delegate<TIn, TParameter, TResult>(this ISelect<TIn, ISelect<TParameter, TResult>> @this)
			=> @this.Out(DelegateSelector<TParameter, TResult>.Default);*/
	}
}
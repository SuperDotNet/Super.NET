﻿using Super.Model.Collections;
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

		public static ISelect<TIn, TTo> CastForValue<TIn, TOut, TTo>(this ISelect<TIn, TOut> @this, I<TTo> _)
			=> @this.Select(ValueAwareCast<TOut, TTo>.Default);

		public static ISelect<TIn, TNew> New<TIn, TOut, TNew>(this ISelect<TIn, TOut> @this, I<TNew> _)
			=> @this.Select(Activations<TOut, TNew>.Default.ToDelegate());

		public static ISelect<TIn, TNew> Activate<TIn, TOut, TNew>(this ISelect<TIn, TOut> @this, I<TNew> _)
			where TNew : IActivateMarker<TOut>
			=> @this.Select(MarkedActivations<TOut, TNew>.Default.ToDelegate());

		public static ISource<TTo> Reduce<TFrom, TTo>(this ISelect<TFrom, TTo> @this) => New<TFrom>.Default.Select(@this);

		public static ISelect<TIn, Type> Type<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> @this.Select(InstanceTypeSelector<TOut>.Default);

		public static ISelect<T, TypeInfo> Metadata<T>(this ISelect<T, Type> @this)
			=> @this.Select(TypeMetadataSelector.Default);

		public static ISelect<TIn, TOut> OnceStriped<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> OncePerParameter<TIn, TOut>.Default.Get(@this);

		public static ISelect<TIn, TOut> OnlyOnce<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> OnlyOnceAlteration<TIn, TOut>.Default.Get(@this);

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
	}
}
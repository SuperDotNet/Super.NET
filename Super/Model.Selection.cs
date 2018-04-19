using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Selection.Stores;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Reflection.Types;
using Super.Runtime;
using Super.Runtime.Activation;
using Super.Runtime.Objects;
using Super.Text;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reactive;

namespace Super
{
	public static partial class ExtensionMethods
	{
		public static ISelect<TParameter, TIn, TOut> Allow<TParameter, TIn, TOut>(this Func<TIn, TOut> @this, I<TParameter> infer)
			=> I<Select<TParameter, TIn, TOut>>.Default.From(ToDelegate(Allow(@this.ToSource(), infer)));

		public static ISelect<T, T> Guard<T>(this IMessage<object> @this)
			=> If(new AssignedInstanceGuard<T>(@this.In(Cast<T>.Default)), Model.Selection.Self<T>.Default);

		public static ISelect<TParameter, TResult> Guard<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this) => @this.Or(GuardedFallback<TParameter, TResult>.Default);

		public static ISelect<TParameter, TResult> Guard<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, Func<TParameter, string> message)
			=> Guard(@this, new Message<TParameter>(message));

		public static ISelect<TParameter, TResult> Guard<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, IMessage<TParameter> message)
			=> @this.Or(new GuardedFallback<TParameter, TResult>(message));

		public static ISelect<TParameter, TResult> Try<TException, TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, I<TException> infer) where TException : Exception
			=> Try(infer, ToDelegate(@this), ToDelegate(Default(@this)));

		public static ISelect<TParameter, TResult> Default<TParameter, TResult>(this ISelect<TParameter, TResult> @this)
			=> Defaults<TParameter, TResult>.Default.Get(@this);

		public static ISelect<TParameter, TResult> Or<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISource<TResult> source)
			=> @this.Or(Allow(source, I<TParameter>.Default));

		public static ISelect<TParameter, TResult> Or<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISelect<TParameter, TResult> next)
			=> Or(@this, IsAssigned<TResult>.Default, next);

		public static ISelect<TParameter, TResult> Or<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISpecification<TResult> specification,
			ISelect<TParameter, TResult> fallback)
			=> ToDelegate(@this).Or(specification.ToDelegate(), ToDelegate(fallback));

		public static ISelect<TParameter, TResult> Or<TParameter, TResult>(
			this Func<TParameter, TResult> @this,
			Func<TParameter, TResult> fallback) => @this.Or(IsAssigned<TResult>.Default.ToDelegate(), fallback);

		public static ISelect<TParameter, TResult> Or<TParameter, TResult>(
			this Func<TParameter, TResult> @this, Func<TResult, bool> specification,
			Func<TParameter, TResult> fallback)
			=> new ValidatedResult<TParameter, TResult>(specification, @this, fallback);

		public static KeyValuePair<TParameter, Func<TIn, TOut>> Pair<TParameter, TIn, TOut>(
			this ISelect<TIn, TOut> @this, TParameter parameter, Func<TIn, TOut> select)
			=> Pairs.Create(parameter, select);

		public static ISelect<TSelect, Func<TParameter, TResult>> Unless<TSelect, TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISpecification<TSelect> select, ISelect<TParameter, TResult> then)
			=> Allow(ToDelegate(@this)
				                                                   .ToSource(), I<TSelect>.Default)
			        .Unless(select, then);

		public static ISelect<TSelect, Func<TParameter, TResult>> Unless<TSelect, TParameter, TResult>(
			this ISelect<TSelect, Func<TParameter, TResult>> @this, ISpecification<TSelect> select,
			ISelect<TParameter, TResult> then)
			=> Unless<TSelect, Func<TParameter, TResult>>(@this, select,
			                                                    Allow(ToDelegate(then).ToSource(), I<TSelect>.Default));

		public static ISelect<TParameter, TResult> Unless<TParameter, TResult, TOther>(
			this ISelect<TParameter, TResult> @this, ISelect<TOther, TResult> then)
			=> Unless<TParameter, TResult>(@this, IsType<TParameter, TOther>.Default,
			                                     then.In(Cast<TParameter>.Default));

		public static ISelect<TParameter, TResult> Unless<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISpecification<TParameter> specification, ISelect<TParameter, TResult> then)
			=> If(specification, then, @this);

		public static ISelect<TParameter, TResult> Configure<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, IAssignable<TParameter, TResult> assignable)
			=> new Configuration<TParameter, TResult>(@this, assignable);

		public static ISelect<TParameter, TResult> Assigned<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this) => If(IsAssigned<TParameter>.Default, @this);

		public static T Get<T>(this ISelect<Unit, T> @this) => @this.Get(Unit.Default);

		public static Func<TParameter, TResult> ToSelect<TParameter, TResult>(this ISpecification<TParameter, TResult> @this)
			=> Delegates<TParameter, TResult>.Default.Get(@this);

		public static Func<TParameter, bool> ToPredicate<TParameter, TResult>(this ISpecification<TParameter, TResult> @this)
			=> Model.Specifications.Delegates<TParameter>.Default.Get(@this);

		public static Func<TParameter, TResult> ToDelegate<TParameter, TResult>(this ISelect<TParameter, TResult> @this)
			=> Delegates<TParameter, TResult>.Default.Get(@this);

		public static ISelect<TParameter, TResult> ToStore<TParameter, TResult>(this ISelect<TParameter, TResult> @this)
			where TParameter : class => ToDelegate(@this).ToStore();

		public static ISelect<TParameter, TResult> ToStore<TParameter, TResult>(this Func<TParameter, TResult> @this)
			where TParameter : class => ReferenceStores<TParameter, TResult>.Default.Get(@this);


		public static ISelect<Unit, T> ToSelect<T>(this T @this) => @this.ToSelect(I<Unit>.Default);

		public static ISelect<TParameter, TResult> ToSelect<TParameter, TResult>(this TResult @this, I<TParameter> _)
			=> new FixedResult<TParameter, TResult>(@this);

		public static ISelect<TParameter, TResult> ToSelect<TParameter, TResult>(this Func<TParameter, TResult> @this)
			=> Selections<TParameter, TResult>.Default.Get(@this);

		public static ISelect<TParameter, TResult> ToSelect<TParameter, TResult, TAttribute>(
			this TResult @this, I<TAttribute> _) where TAttribute : Attribute
			=> @this.OrDefault(IsDefined<TAttribute>.Default.Select(InstanceMetadataSelector<TParameter>.Default));

		public static ISpecification<TParameter, TResult> ToSpecification<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this,
			ISpecification<TParameter> specification)
			=> new Specification<TParameter, TResult>(specification, @this);

		public static TResult Get<TItem, TResult>(this ISelect<ImmutableArray<TItem>, TResult> @this,
		                                          params TItem[] parameters)
			=> @this.Get(parameters.ToImmutableArray());

		public static ISelect<TParameter, TResult> When<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISpecification<TResult> specification)
			=> Or(@this, specification, Default(@this));

		public static ISelect<TParameter, TResult> OrDefault<TParameter, TResult>(
			this TResult @this, ISpecification<TParameter> specification)
			=> new Conditional<TParameter, TResult>(specification, @this, default);
	}
}
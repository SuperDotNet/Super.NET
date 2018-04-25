using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Selection.Stores;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Reflection.Types;
using Super.Runtime;
using Super.Runtime.Activation;
using Super.Text;
using System;
using System.Collections.Immutable;

namespace Super
{
	public static partial class ExtensionMethods
	{
		public static ISelect<TParameter, TResult> AsSelect<TParameter, TResult>(this ISelect<TParameter, TResult> @this)
			=> @this;

		public static ISelect<TParameter, TIn, TOut> Allow<TParameter, TIn, TOut>(
			this Func<TIn, TOut> @this, I<TParameter> infer)
			=> I<Select<TParameter, TIn, TOut>>.Default.From(@this.ToSource().Allow(infer).ToDelegate());

		public static ISelect<TParameter, TResult> Guard<TParameter, TResult>(this ISelect<TParameter, TResult> @this)
			=> @this.Guard(DefaultMessage<TParameter, TResult>.Default);

		public static ISelect<TParameter, TResult> Guard<TParameter, TResult>(this ISelect<TParameter, TResult> @this, IMessage<TParameter> message)
			=> new AssignedInstanceGuard<TParameter>(message).If(@this);

		/*public static ISelect<TParameter, TResult> Guard<TParameter, TResult>(this ISelect<TParameter, TResult> @this,
		                                                                      Func<TParameter, string> message)
			=> @this.Guard(new Message<TParameter>(message));

		public static ISelect<TParameter, TResult> Guard<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this) => @this.Or(GuardedFallback<TParameter, TResult>.Default);

		public static ISelect<TParameter, TResult> Guard<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, IMessage<TParameter> message)
			=> @this.Or(new GuardedFallback<TParameter, TResult>(message));*/

		public static ISelect<TParameter, TResult> Try<TException, TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, I<TException> infer) where TException : Exception
			=> infer.Try(@this.ToDelegate(), @this.Default().ToDelegate());

		public static ISelect<TParameter, TResult> Default<TParameter, TResult>(this ISelect<TParameter, TResult> @this)
			=> Defaults<TParameter, TResult>.Default.Get(@this);

		public static ISelect<TParameter, TResult> OrGuard<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this)
			=> @this.Or(GuardedFallback<TParameter, TResult>.Default);

		public static ISelect<TParameter, TResult> Or<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, IMessage<TParameter> message)
			=> @this.Or(new GuardedFallback<TParameter, TResult>(message));

		public static ISelect<TParameter, TResult> Or<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISource<TResult> source)
			=> @this.Or(source.Allow(I<TParameter>.Default));

		public static ISelect<TParameter, TResult> Or<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISelect<TParameter, TResult> next)
			=> @this.Or(IsAssigned<TResult>.Default, next);

		public static ISelect<TParameter, TResult> Or<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISpecification<TResult> specification,
			ISelect<TParameter, TResult> fallback)
			=> @this.ToDelegate().Or(specification.IsSatisfiedBy, fallback.Get);

		public static ISelect<TParameter, TResult> Or<TParameter, TResult>(
			this Func<TParameter, TResult> @this, Func<TResult, bool> specification,
			Func<TParameter, TResult> fallback)
			=> new ValidatedResult<TParameter, TResult>(specification, @this, fallback);

		public static ISelect<TSelect, Func<TParameter, TResult>> Unless<TSelect, TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISpecification<TSelect> select, ISelect<TParameter, TResult> then)
			=> @this.ToDelegate().ToSource().Allow(I<TSelect>.Default).Unless(select, then);

		public static ISelect<TSelect, Func<TParameter, TResult>> Unless<TSelect, TParameter, TResult>(
			this ISelect<TSelect, Func<TParameter, TResult>> @this, ISpecification<TSelect> select,
			ISelect<TParameter, TResult> then)
			=> @this.Unless<TSelect, Func<TParameter, TResult>>(select, then.ToDelegate().ToSource().Allow(I<TSelect>.Default));

		public static ISelect<TParameter, TResult> Unless<TParameter, TResult, TOther>(
			this ISelect<TParameter, TResult> @this, ISelect<TOther, TResult> then)
			=> @this.Unless<TParameter, TResult>(IsType<TParameter, TOther>.Default, In<TParameter>.Cast(then));

		public static ISelect<TParameter, TResult> Unless<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISpecification<TParameter> specification, ISelect<TParameter, TResult> then)
			=> specification.If(then, @this);

		public static ISelect<TParameter, TResult> Configure<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, IAssignable<TParameter, TResult> assignable)
			=> new Configuration<TParameter, TResult>(@this, assignable);

		public static ISelect<TParameter, TResult> Assigned<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this) => IsAssigned<TParameter>.Default.If(@this);

		public static Func<TParameter, TResult> ToDelegate<TParameter, TResult>(this ISelect<TParameter, TResult> @this)
			=> @this.Get;

		public static Func<TParameter, TResult> ToDelegateReference<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this) => Delegates<TParameter, TResult>.Default.Get(@this);

		public static ISelect<TParameter, TResult> ToStore<TParameter, TResult>(this ISelect<TParameter, TResult> @this)
			where TParameter : class => @this.ToDelegateReference().ToStore();

		public static ISelect<TParameter, TResult> ToStore<TParameter, TResult>(this Func<TParameter, TResult> @this)
			where TParameter : class => ReferenceTables<TParameter, TResult>.Default.Get(@this);

		internal static ISelect<TParameter, TResult> ToReferenceStore<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this)
			where TResult : class
			where TParameter : class => new ReferenceValueTable<TParameter, TResult>(@this.Get);

		/*public static ISelect<TParameter, TResult> ToSelect<TParameter, TResult, TAttribute>(
			this TResult @this, I<TAttribute> _) where TAttribute : Attribute
			=> @this.OrDefault(IsDefined<TAttribute>.Default.Select(InstanceMetadataSelector<TParameter>.Default));*/

		public static ISpecification<TParameter, TResult> ToSpecification<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISpecification<TParameter> specification)
			=> new Specification<TParameter, TResult>(specification, @this);

		public static TResult Get<TItem, TResult>(this ISelect<ImmutableArray<TItem>, TResult> @this,
		                                          params TItem[] parameters)
			=> @this.Get(parameters.ToImmutableArray());

		public static ISelect<TParameter, TResult> When<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISpecification<TResult> specification)
			=> @this.Or(specification, @this.Default());

		public static ISelect<TParameter, TResult> OrDefault<TParameter, TResult>(
			this TResult @this, ISpecification<TParameter> specification)
			=> new Conditional<TParameter, TResult>(specification, @this);
	}
}
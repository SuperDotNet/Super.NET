using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Selection.Stores;
using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Reflection.Types;
using Super.Runtime;
using Super.Text;
using System;
using System.Collections.Immutable;
using System.Reactive;

namespace Super
{
	public static partial class ExtensionMethods
	{
		public static ISelect<TParameter, TResult> AsSelect<TParameter, TResult>(this ISelect<TParameter, TResult> @this)
			=> @this;

		public static ISelect<TParameter, TResult> Default<TParameter, TResult>(this ISelect<TParameter, TResult> @this)
			=> Defaults<TParameter, TResult>.Default.Get(@this);

		public static ISelect<TParameter, TIn, TOut> Allow<TParameter, TIn, TOut>(
			this Func<TIn, TOut> @this, I<TParameter> infer)
			=> I<Select<TParameter, TIn, TOut>>.Default.From(@this.ToSource().Allow(infer).ToDelegate());

		public static ISelect<T, TOut> Allow<T, TOut>(this ISelect<Unit, TOut> @this, I<T> _)
			=> In<T>.Select(__ => Unit.Default).Select(@this);

		public static ISelect<TParameter, TResult> Guard<TParameter, TResult>(this ISelect<TParameter, TResult> @this)
			=> @this.Guard(DefaultMessage<TParameter, TResult>.Default);

		public static ISelect<TParameter, TResult> Guard<TParameter, TResult>(this ISelect<TParameter, TResult> @this,
		                                                                      IMessage<TParameter> message)
			=> new Guard<TParameter>(message).If(@this);

		public static ISelect<TIn, TOut> If<TIn, TOut>(this ISelect<TIn, TOut> @this, ISpecification<TIn> specification)
			=> new Validated<TIn, TOut>(specification.IsSatisfiedBy, @this.Get);

		public static ISelect<TParameter, TResult> If<TParameter, TResult>(
			this ISpecification<TParameter> @this, ISelect<TParameter, TResult> @true) => @this.If(@true, @true.Default());

		public static ISelect<TParameter, TResult> If<TParameter, TResult>(
			this ISpecification<TParameter> @this, ISelect<TParameter, TResult> @true, ISelect<TParameter, TResult> @false)
			=> new Validated<TParameter, TResult>(@this, @true, @false);

		public static ISelect<TParameter, TResult> Try<TException, TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, I<TException> _) where TException : Exception
			=> new Try<TException, TParameter, TResult>(@this.ToDelegate(), @this.Default().ToDelegate());

		/*public static ISelect<TParameter, TResult> Or<TParameter, TResult>(this ISource<TResult> @this,
		                                                                   ISelect<TParameter, TResult> select)
			=> @this.Allow(I<TParameter>.Default).Assigned(select);*/

		public static ISelect<TParameter, TResult> Assigned<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISelect<TParameter, TResult> next)
			=> @this.When(IsAssigned<TResult>.Default, next.Get);

		public static ISelect<TParameter, TResult> Or<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISelect<TResult, string> message)
			=> @this.When(message.To(I<Guard<TResult>>.Default));

		public static ISelect<TParameter, TResult> Or<TParameter, TResult>(this ISelect<TParameter, TResult> @this,
		                                                                   Func<TResult> source)
			=> @this.Or(source.ToSource());

		public static ISelect<TParameter, TResult> Or<TParameter, TResult>(this ISelect<TParameter, TResult> @this,
		                                                                   ISource<TResult> source)
			=> @this.Assigned(source.Allow(I<TParameter>.Default));

		public static ISelect<TParameter, TResult> Assigned<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this) => IsAssigned<TParameter>.Default.If(@this);

		public static ISelect<TParameter, TResult> When<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISpecification<TResult> specification)
			=> @this.When(specification, @this.Default().Get);

		public static ISelect<TParameter, TResult> When<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISpecification<TResult> specification, Func<TParameter, TResult> select)
			=> new ValidatedResult<TParameter, TResult>(specification.IsSatisfiedBy, @this.Get, select);


		public static ISelect<TParameter, TResult> Unless<TParameter, TResult>(this ISelect<TParameter, TResult> @this,
		                                                                       ISpecification<TParameter, TResult> then)
			=> @this.Unless(then, then);

		public static ISelect<TParameter, TResult> Unless<TParameter, TResult, TOther>(
			this ISelect<TParameter, TResult> @this, ISelect<TOther, TResult> then)
			=> @this.Unless(IsType<TParameter, TOther>.Default, In<TParameter>.Cast(then));

		public static ISelect<TParameter, TResult> Unless<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISpecification<TParameter> specification, ISelect<TParameter, TResult> then)
			=> specification.If(then, @this);



		public static ISelect<TParameter, TResult> Configure<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, IAssignable<TParameter, TResult> assignable)
			=> new Configuration<TParameter, TResult>(@this, assignable);

		/*public static ISelect<TParameter, TResult> Assigned<TParameter, TResult, TTo>(
			this ISelect<TParameter, TResult> @this, Func<TResult, TTo> select) => @this.Out(IsAssigned<TResult>.Default.If(select).ToDelegate());*/

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

		/*public static ISpecification<TParameter, TResult> ToSpecification<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISpecification<TParameter> specification)
			=> new Specification<TParameter, TResult>(specification, @this);*/

		public static TResult Get<TItem, TResult>(this ISelect<ImmutableArray<TItem>, TResult> @this,
		                                          params TItem[] parameters)
			=> @this.Get(parameters.ToImmutableArray());
	}
}
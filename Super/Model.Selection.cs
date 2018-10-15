using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Selection.Stores;
using Super.Model.Sequences;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Reflection.Types;
using Super.Runtime;
using Super.Text;
using System;
using System.Collections.Immutable;

namespace Super
{
	// ReSharper disable once MismatchedFileName
	public static partial class ExtensionMethods
	{
		public static T Get<T>(this ISelect<uint, T> @this, int parameter) => @this.Get((uint)parameter);

		public static T Get<T>(this ISelect<int, T> @this, uint parameter) => @this.Get((int)parameter);

		public static T Invoke<T>(this Func<uint, T> @this, int parameter) => @this((uint)parameter);

		public static ISelect<TIn, TOut> AsSelect<TIn, TOut>(this ISelect<TIn, TOut> @this) => @this;

		//public static ISelect<Unit, T> AsSelect<T>(this IAny<T> @this) => @this;

		public static ISelect<TParameter, TResult> If<TParameter, TResult>(this ISelect<TParameter, TResult> @this,
		                                                                   ISpecification<TParameter> @true)
			=> Default<TParameter, TResult>.Instance.Unless(@true, @this);

		public static ISelect<TParameter, TResult> Then<TParameter, TResult>(this ISpecification<TParameter> @this,
		                                                                     ISelect<TParameter, TResult> select)
			=> select.If(@this);

		public static ISelect<TParameter, TResult> Guard<TParameter, TResult>(this ISelect<TParameter, TResult> @this)
			=> DefaultGuard<TParameter>.Default.Then(@this);

		public static ISelect<TParameter, TResult> Guard<TParameter, TResult>(this ISelect<TParameter, TResult> @this,
		                                                                      IMessage<TParameter> message)
			=> new AssignedGuard<TParameter>(message).Then(@this);

		public static ISelect<TParameter, TResult> Assigned<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this)
			=> IsAssigned<TParameter>.Default.Then(@this);

		public static ISelect<TParameter, TResult> Unless<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISpecification<TParameter, TResult> then)
			=> @this.Unless(then, then);

		public static ISelect<TParameter, TResult> UnlessCast<TParameter, TResult, TOther>(
			this ISelect<TParameter, TResult> @this, ISelect<TOther, TResult> then)
			=> @this.Unless(IsType<TParameter, TOther>.Default,
			                Runtime.Objects.Cast<TParameter, TOther>.Default.Select(then));

		public static ISelect<TParameter, TResult> Unless<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISpecification<TParameter> specification,
			ISelect<TParameter, TResult> then)
			=> new Validated<TParameter, TResult>(specification, then, @this);

		public static ISelect<TParameter, TResult> Unless<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISelect<TParameter, TResult> then)
			=> @this.UnlessResult(IsAssigned<TResult>.Default, then);

		public static ISelect<TParameter, TResult> UnlessResult<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, ISpecification<TResult> specification,
			ISelect<TParameter, TResult> then)
			=> new ValidatedResult<TParameter, TResult>(specification, then, @this);

		public static ISelect<TParameter, TResult> Try<TException, TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, I<TException> _) where TException : Exception
			=> new Try<TException, TParameter, TResult>(@this.ToDelegate());

		public static ISelect<TParameter, TResult> Configure<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this, IAssignable<TParameter, TResult> assignable)
			=> new Configuration<TParameter, TResult>(@this, assignable);

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

		public static TResult Get<TItem, TResult>(this ISelect<ImmutableArray<TItem>, TResult> @this,
		                                          params TItem[] parameters)
			=> @this.Get(parameters.ToImmutableArray());

		public static TResult Get<TItem, TResult>(this ISelect<ImmutableArray<TItem>, TResult> @this,
		                                          TItem parameter)
			=> @this.Get(ImmutableArray.Create(parameter));

		public static TResult Get<TItem, TResult>(this ISelect<Array<TItem>, TResult> @this,
		                                          params TItem[] parameters) => @this.Get(parameters);

		public static TResult Get<TItem, TResult>(this ISelect<Array<TItem>, TResult> @this,
		                                          TItem parameter) => @this.Get(new[] {parameter});

		/**/

		public static ISelect<_, ISpecification<TFrom, __>> Select<_, __, TFrom, TTo>(
			this ISelect<_, ISpecification<TTo, __>> @this, ISelect<TFrom, TTo> select)
			=> @this.Select<_, __, TFrom, TTo>(select.Get);

		public static ISelect<_, ISpecification<TFrom, __>> Select<_, __, TFrom, TTo>(
			this ISelect<_, ISpecification<TTo, __>> @this, Func<TFrom, TTo> select)
			=> @this.Select(new ParameterSelection<__, TFrom, TTo>(select));

		public static ISpecification<TFrom, TOut> Select<TFrom, TTo, TOut>(this ISpecification<TTo, TOut> @this,
		                                                                   ISelect<TFrom, TTo> select)
			=> @this.Select<TFrom, TTo, TOut>(select.Get);

		public static ISpecification<TFrom, TOut> Select<TFrom, TTo, TOut>(this ISpecification<TTo, TOut> @this,
		                                                                   Func<TFrom, TTo> select)
			=> new SelectedParameterSpecification<TFrom, TTo, TOut>(select, @this);

		public static ISelect<_, Func<T, bool>> ToDelegate<_, T, __>(this ISelect<_, ISpecification<T, __>> @this)
			=> @this.Select(x => x.AsSpecification().ToDelegate());
	}
}
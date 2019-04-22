using Super.Compose;
using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Selection.Adapters;
using Super.Model.Selection.Conditions;
using Super.Model.Sequences;
using Super.Reflection;
using Super.Runtime;
using Super.Runtime.Execution;
using System;

namespace Super
{
	// ReSharper disable once MismatchedFileName
	public static partial class ExtensionMethods
	{
		public static IResult<T> AsResult<T>(this IResult<T> @this) => @this;

		public static IResult<ISelect<TIn, TOut>> AsDefined<TIn, TOut>(this IResult<ISelect<TIn, TOut>> @this) => @this;

		public static IResult<T> Start<T>(this T @this) => Compose.Start.A.Result(@this);

		public static IResult<T> Start<T>(this Func<T> @this) => @this.Target as IResult<T> ??
		                                                         I.A<Model.Results.Result<T>>().From(@this);

		public static IResult<T> Singleton<T>(this IResult<T> @this) => new DeferredSingleton<T>(@this.Get);

		public static IResult<T> Unless<T>(this IResult<T> @this, IResult<T> assigned)
			=> @this.Unless(IsAssigned<T>.Default, assigned);

		public static IResult<T> Unless<T>(this IResult<T> @this, ICondition<T> condition, IResult<T> then)
			=> @this.Unless(condition.ToDelegate(), then);

		public static IResult<T> Unless<T>(this IResult<T> @this, Model.Selection.Adapters.Condition<T> condition, IResult<T> then)
			=> new ValidatedResult<T>(condition, then, @this);

		public static IResult<T> Unless<T>(this IResult<T> @this, ICondition condition, IResult<T> then)
			=> @this.Unless(condition.ToResult().ToDelegate(), then);

		public static IResult<T> Unless<T>(this IResult<T> @this, Condition condition, IResult<T> then)
			=> new Validated<T>(condition, then, @this);

		public static IResult<T> Emit<T>(this IResult<Func<T>> @this) => new WrappedResult<T>(@this.Get);

		public static ISelect<TIn, TOut> Emit<TIn, TOut>(this IResult<Func<TIn, TOut>> @this)
			=> new DelegatedInstanceSelector<TIn, TOut>(@this.Get);

		public static ISelect<TIn, TOut> Emit<TIn, TOut>(this IResult<ISelect<TIn, TOut>> @this)
			=> @this.ToSelect().Then().Delegate().Out().Start().Emit();

		public static IResult<T> ToContextual<T>(this IResult<T> @this)
			=> new Contextual<T>(@this.ToDelegateReference());

		/**/

		public static IResult<TOut> Select<TIn, TOut>(this IResult<TIn> @this, ISelect<TIn, TOut> select)
			=> @this.Select(@select.Get);

		public static IResult<TOut> Select<TIn, TOut>(this IResult<TIn> @this, Func<TIn, TOut> select)
			=> new DelegatedSelection<TIn, TOut>(select, @this.Get);

		public static IResult<Array<TTo>> Select<TFrom, TTo>(this IResult<Array<TFrom>> @this, Func<TFrom, TTo> select)
			=> @this.Query()
			        .Select(select)
			        .Get()
			        .Out();

		/**/

		public static Func<T> ToDelegate<T>(this IResult<T> @this) => @this.Get;

		public static Func<T> ToDelegateReference<T>(this IResult<T> @this)
			=> Model.Results.Delegates<T>.Default.Get(@this);

		public static ISelect<TIn, TOut> ToSelect<TIn, TOut>(this IResult<TOut> @this, I<TIn> _)
			=> Compose.Start.A.Selection<TIn>().By.Returning(@this);

		public static ISelect<T> ToSelect<T>(this IResult<T> @this) => new Model.Selection.Adapters.Result<T>(@this.Get);
	}
}
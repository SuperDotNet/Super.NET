using Super.Compose;
using Super.Model.Commands;
using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Selection.Adapters;
using Super.Model.Selection.Alterations;
using Super.Model.Selection.Conditions;
using Super.Model.Selection.Stores;
using Super.Model.Sequences;
using Super.Reflection;
using Super.Reflection.Types;
using Super.Runtime;
using Super.Runtime.Activation;
using Super.Runtime.Objects;
using Super.Text;
using System;
using None = Super.Runtime.None;

namespace Super
{
	// ReSharper disable once MismatchedFileName
	public static partial class ExtensionMethods
	{
		public static T Get<T>(this ISelect<uint, T> @this, int parameter) => @this.Get((uint)parameter);

		public static TOut Get<TItem, TOut>(this ISelect<Array<TItem>, TOut> @this, TItem parameter)
			=> @this.Get(Model.Sequences.Query.Yield<TItem>.Default.Get(parameter));

		public static TOut Get<TItem, TOut>(this ISelect<Array<TItem>, TOut> @this, params TItem[] parameters)
			=> @this.Get(parameters);

		public static T Get<T>(this ISelect<None, T> @this) => @this.Get(None.Default);

		public static IResult<T> Out<T>(this ISelect<None, T> @this) => @this.In(None.Default);

		public static IResult<TOut> New<TIn, TOut>(this ISelect<TIn, TOut> @this) => @this.In(New<TIn>.Default);

		/**/

		public static ISelect<TIn, TOut> Guard<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> DefaultGuard<TIn>.Default.Then().ToConfiguration().Select(@this).Get();

		public static IAlteration<T> AsGuard<T>(this IMessage<T> @this) => new AssignedGuard<T>(@this).Then().Out();

		/**/

		public static ISelect<TIn, TOut> Select<TIn, TOut>(this ISelect<TIn, TOut> @this,
		                                                   ISelect<Decoration<TIn, TOut>, TOut> other)
			=> new Decorator<TIn, TOut>(other, @this);

		public static ISelect<TIn, TTo> Select<TIn, TFrom, TTo>(this ISelect<TIn, TFrom> @this,
		                                                        ISelect<TFrom, TTo> select) => @this.Select(select.Get);

		public static ISelect<TIn, TTo> Select<TIn, TFrom, TTo>(this ISelect<TIn, TFrom> @this, Func<TFrom, TTo> select)
			=> new Selection<TIn, TFrom, TTo>(@this.Get, select);

		public static ISelect<TIn, TOut> SelectOf<TIn, TOut, TOther>(this ISelect<TIn, TOut> @this,
		                                                             IAssign<TIn, TOther> configuration)
			=> new Configuration<TIn, TOut, TOther>(@this, configuration);

		public static ISelect<TIn, TOut> Select<TIn, TOut>(this ISelect<TIn, TOut> @this,
		                                                   IAssign<TIn, TOut> configuration)
			=> new Configuration<TIn, TOut>(@this, configuration);

		public static ISelect<_, IConditional<TFrom, __>> Select<_, __, TFrom, TTo>(
			this ISelect<_, IConditional<TTo, __>> @this, ISelect<TFrom, TTo> select)
			=> @this.Select(new ParameterSelection<__, TFrom, TTo>(select.Get));

/**/

		public static IResult<TOut> In<TIn, TOut>(this ISelect<TIn, TOut> @this, TIn parameter)
			=> new FixedSelection<TIn, TOut>(@this, parameter);

		public static IResult<TOut> In<TIn, TOut>(this ISelect<TIn, TOut> @this, Func<TIn> parameter)
			=> new DelegatedSelection<TIn, TOut>(@this.Get, parameter);

		public static IResult<TOut> In<TIn, TOut>(this ISelect<TIn, TOut> @this, IResult<TIn> parameter)
			=> new DelegatedSelection<TIn, TOut>(@this, parameter);

/**/

		public static ISelect<TIn, TOut> Assigned<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> @this.If(A.Of<IsAssigned<TIn>>());

		public static ISelect<TIn, TOut> If<TIn, TOut>(this ISelect<TIn, TOut> @this, ISelect<TIn, bool> @true)
			=> Compose.Start.A.Selection<TIn>().By.Default<TOut>().Unless(@true, @this);

		public static ISelect<TIn, TOut> Unless<TIn, TOut>(this ISelect<TIn, TOut> @this, ISelect<TIn, TOut> assigned)
			=> @this.Unless(assigned.ToDelegate());

		public static ISelect<TIn, TOut> Unless<TIn, TOut>(this ISelect<TIn, TOut> @this, Selection<TIn, TOut> assigned)
			=> new ValidatedResult<TIn, TOut>(IsAssigned<TOut>.Default, assigned, @this);

		public static ISelect<TIn, TOut> Unless<TIn, TOut>(this ISelect<TIn, TOut> @this, IConditional<TIn, TOut> then)
			=> @this.Unless(then.Condition, then);

		public static ISelect<TIn, TOut> UnlessOf<TIn, TOut, T>(this ISelect<TIn, TOut> @this, ISelect<T, TOut> then)
			=> @this.Unless(IsOf<TIn, T>.Default, A.Of<CastOrThrow<TIn, T>>().Select(then));

		public static ISelect<TIn, TOut> Unless<TIn, TOut>(this ISelect<TIn, TOut> @this, ICondition<TIn> condition,
		                                                   IResult<TOut> then)
			=> @this.Unless(condition, then.ToSelect(I.A<TIn>()));

		public static ISelect<TIn, TOut> Unless<TIn, TOut>(this ISelect<TIn, TOut> @this, ISelect<TIn, bool> unless,
		                                                   ISelect<TIn, TOut> then)
			=> new Validated<TIn, TOut>(unless.Get, then.Get, @this.Get);

		/**/

		public static Func<TIn, TOut> ToDelegate<TIn, TOut>(this ISelect<TIn, TOut> @this) => @this.Get;

		public static Func<TIn, TOut> ToDelegateReference<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> Delegates<TIn, TOut>.Default.Get(@this);

		public static ISelect<TIn, TOut> ToStore<TIn, TOut>(this ISelect<TIn, TOut> @this) where TIn : class
			=> ReferenceTables<TIn, TOut>.Default.Get(@this.ToDelegateReference());

		public static ISelect<TIn, TOut> ToSelect<TIn, TOut>(this Func<TIn, TOut> @this)
			=> Selections<TIn, TOut>.Default.Get(@this);

		public static ICondition<T> ToCondition<T>(this ISelect<T, bool> @this) => @this as ICondition<T> ??
		                                                                           new Model.Selection.Conditions.Condition<T>(@this.Get);

		public static IConditional<TIn, TOut> ToConditional<TIn, TOut>(this ISelect<TIn, TOut> @this,
		                                                               ICondition<TIn> condition)
			=> new Conditional<TIn, TOut>(condition, @this.Get);

		public static ICommand<TIn> Terminate<TIn, TOut>(this ISelect<TIn, TOut> @this, ICommand<TOut> command)
			=> new SelectedParameterCommand<TIn, TOut>(command.Execute, @this.Get);

		public static ICommand<T> ToCommand<T>(this ISelect<T, None> @this) => new InvokeParameterCommand<T>(@this.Get);

		public static ICommand ToAction(this ISelect<None, None> @this)
			=> new Model.Selection.Adapters.Action(@this.ToCommand().Execute);

		public static IResult<T> ToResult<T>(this ISelect<None, T> @this) => @this as IResult<T> ??
		                                                                     new DelegatedResult<T>(@this.Get);
	}
}
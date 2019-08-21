﻿using Super.Model.Commands;
using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Selection.Adapters;
using Super.Model.Selection.Alterations;
using Super.Model.Selection.Conditions;
using Super.Model.Sequences;
using Super.Model.Sequences.Query;
using Super.Operations;
using Super.Runtime;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Action = System.Action;

namespace Super
{
	// ReSharper disable once MismatchedFileName
	public static partial class ExtensionMethods
	{
		public static ISelect<TIn, IResult<TOut>> AsDefined<TIn, TOut>(this ISelect<TIn, IResult<TOut>> @this) => @this;

		public static ConditionSelector<_, T> Then<_, T>(this ISelect<_, ICondition<T>> @this)
			=> new ConditionSelector<_, T>(@this);

		public static OperationSelector<_, T> Then<_, T>(this ISelect<_, ValueTask<T>> @this)
			=> new OperationSelector<_, T>(@this);

		public static TaskSelector<_, T> Then<_, T>(this ISelect<_, Task<T>> @this) => new TaskSelector<_, T>(@this);

		public static Selector<TIn, TOut> Then<TIn, TOut>(this ISelect<TIn, TOut> @this)
			=> new Selector<TIn, TOut>(@this);

		public static ConditionSelector<T> Then<T>(this ISelect<T, bool> @this) => new ConditionSelector<T>(@this);

		public static TypeSelector<T> Then<T>(this ISelect<T, Type> @this) => new TypeSelector<T>(@this);

		public static ResultSelector<T> Then<T>(this IResult<T> @this) => new ResultSelector<T>(@this.ToSelect());

		public static CommandSelector Then(this ICommand @this) => new CommandSelector(@this);

		public static CommandSelector<T> Then<T>(this ICommand<T> @this) => new CommandSelector<T>(@this);

		public static CommandSelector<T> Then<T>(this ICommand<T> @this, params ICommand<T>[] others)
			=> new CommandSelector<T>(@this).Then(others);

		public static ResultInstanceSelector<None, T> Then<T>(this IResult<IResult<T>> @this)
			=> new ResultInstanceSelector<None, T>(@this.ToSelect());

		public static CommandInstanceSelector<_, T> Then<_, T>(this ISelect<_, ICommand<T>> @this)
			=> new CommandInstanceSelector<_, T>(@this);

		public static ResultDelegateSelector<_, T> Then<_, T>(this ISelect<_, Func<T>> @this)
			=> new ResultDelegateSelector<_, T>(@this);

		public static ResultInstanceSelector<_, T> Then<_, T>(this ISelect<_, IResult<T>> @this)
			=> new ResultInstanceSelector<_, T>(@this);

		public static SelectionSelector<None, TIn, TOut> Then<TIn, TOut>(this IResult<ISelect<TIn, TOut>> @this)
			=> @this.ToSelect().Then();

		public static SelectionSelector<_, TIn, TOut> Then<_, TIn, TOut>(this ISelect<_, ISelect<TIn, TOut>> @this)
			=> new SelectionSelector<_, TIn, TOut>(@this);

		public static ExpressionSelector<T> Then<T>(this ISelect<T, Expression> @this)
			=> new ExpressionSelector<T>(@this);

		public static CollectionSelector<_, T> Then<_, T>(this ISelect<_, ICollection<T>> @this)
			=> new CollectionSelector<_, T>(@this);

		public static OpenArraySelector<_, T> Then<_, T>(this ISelect<_, T[]> @this)
			=> new OpenArraySelector<_, T>(@this);

		public static Query<_, T> Query<_, T>(this Selector<_, T[]> @this) => new Query<_, T>(@this.Get());

		public static Query<_, T> Query<_, T>(this Selector<_, Array<T>> @this) => @this.Get().Open().Query();

		public static Query<None, T> Query<T>(this IResult<Array<T>> @this) => @this.ToSelect().Query();

		public static Query<None, T> Query<T>(this IResult<IEnumerable<T>> @this) => @this.ToSelect().Query();

		public static Query<_, T> Query<_, T>(this ISelect<_, T[]> @this) => new Query<_, T>(@this);

		public static Query<_, T> Query<_, T>(this ISelect<_, Array<T>> @this) => @this.Open().Query();

		public static Query<_, T> Query<_, T>(this ISelect<_, IEnumerable<T>> @this)
			=> new Query<_, T>(@this.Select(Iterate<T>.Default));

		public static Query<_, T> Query<_, T>(this ISelect<_, ICollection<T>> @this)
			=> new Query<_, T>(@this.Select(CollectionSelection<T>.Default));

		public static Query<_, TOut> Select<_, T, TOut>(this Query<_, T> @this, ISelect<T, TOut> select)
			=> @this.Select(select.ToDelegate());

		public static Query<_, TOut> SelectBy<_, T, TOut>(this Query<_, T> @this, Expression<Func<T, TOut>> select)
			=> @this.Select(new Build.InlineSelect<T, TOut>(select).Returned());

		public static Query<_, TOut> Select<_, T, TOut>(this Query<_, T> @this, Func<T, TOut> select)
			=> @this.Select(new Build.Select<T, TOut>(select).Returned());

		public static Query<_, TOut> SelectManyBy<_, T, TOut>(this Query<_, T> @this,
		                                                      Expression<Func<T, IEnumerable<TOut>>> select)
			=> @this.SelectMany(select.Compile());

		public static Query<_, TOut> SelectMany<_, T, TOut>(this Query<_, T> @this,
		                                                    ISelect<T, IEnumerable<TOut>> select)
			=> @this.SelectMany(select.Get);

		public static Query<_, TOut> SelectMany<_, T, TOut>(this Query<_, T> @this, Func<T, IEnumerable<TOut>> select)
			=> @this.Select(new Build.SelectMany<T, TOut>(select).Returned());

		public static Query<_, T> WhereBy<_, T>(this Query<_, T> @this, Expression<Func<T, bool>> where)
			=> @this.Where(where.Compile());

		public static Query<_, T> Where<_, T>(this Query<_, T> @this, ICondition<T> where) => @this.Where(where.Get);

		public static ISelect<_, T> FirstAssigned<_, T>(this Query<_, T> @this) where T : class
			=> @this.Select(FirstAssigned<T>.Default);

		public static ISelect<_, T?> FirstAssigned<_, T>(this Query<_, T?> @this) where T : struct
			=> @this.Select(FirstAssignedValue<T>.Default);

		public static ISelect<_, int> Sum<_>(this Query<_, int> @this) => @this.Select(Sum32.Default);

		public static ISelect<_, uint> Sum<_>(this Query<_, uint> @this) => @this.Select(SumUnsigned32.Default);

		public static ISelect<_, long> Sum<_>(this Query<_, long> @this) => @this.Select(Sum64.Default);

		public static ISelect<_, ulong> Sum<_>(this Query<_, ulong> @this) => @this.Select(SumUnsigned64.Default);

		public static ISelect<_, float> Sum<_>(this Query<_, float> @this) => @this.Select(SumSingle.Default);

		public static ISelect<_, double> Sum<_>(this Query<_, double> @this) => @this.Select(SumDouble.Default);

		public static ISelect<_, decimal> Sum<_>(this Query<_, decimal> @this) => @this.Select(SumDecimal.Default);

		public static Func<TIn, TOut> Selector<TIn, TOut>(this Selector<TIn, TOut> @this) => @this;

		public static Action Selector(this CommandSelector @this) => @this;

		public static Action Selector(this CommandSelector<None> @this) => @this.Get().Execute;

		public static System.Action<T> Selector<T>(this CommandSelector<T> @this) => @this;

		public static Func<T> Selector<T>(this Selector<None, T> @this) => @this.Get().ToResult().Get;

		public static Func<Array<T>> Selector<T>(this Query<None, T> @this) => @this.Get().ToResult().Get;

		public static ICommand Out<T>(this CommandSelector<T> @this, T parameter) => @this.Input(parameter).Command;

		public static IAlteration<T> Out<T>(this CommandSelector<T> @this)
			=> @this.ToConfiguration().Get().ToAlteration();

		public static ICondition<T> Out<T>(this Selector<T, bool> @this) => @this.Get().ToCondition();

		public static IAlteration<T> Out<T>(this Selector<T, T> @this) => @this.Get().ToAlteration();

		public static ISelect<_, Array<T>> Out<_, T>(this OpenArraySelector<_, T> @this) => @this.Get().Result();
	}
}
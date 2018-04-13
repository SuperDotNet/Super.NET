using Super.Model.Selection;
using Super.Model.Selection.Stores;
using Super.Reflection;
using Super.Runtime.Activation;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Super.ExtensionMethods
{
	partial class Model
	{
		public static ISelect<TParameter, TIn, TOut> ToSelect<TParameter, TIn, TOut>(
			this IEnumerable<KeyValuePair<TParameter, Func<TIn, TOut>>> @this)
			=> @this.ToOrderedDictionary().AsReadOnly().ToStore().ToSelect().To(I<Select<TParameter, TIn, TOut>>.Default);

		public static ISpecification<TParameter, TResult> ToStore<TParameter, TResult>(
			this IDictionary<TParameter, TResult> @this)
			=> @this.To(I<Lookup<TParameter, TResult>>.Default);

		public static ISpecification<TParameter, TResult> ToStore<TParameter, TResult>(
			this IReadOnlyDictionary<TParameter, TResult> @this)
			=> @this.To(I<Lookup<TParameter, TResult>>.Default);

		public static ITable<TParameter, TResult> ToTable<TParameter, TResult>(this IDictionary<TParameter, TResult> @this)
			=> StandardTables<TParameter, TResult>.Default.Get(@this);

		public static ITable<TParameter, TResult> ToTable<TParameter, TResult>(
			this ConcurrentDictionary<TParameter, TResult> @this)
			=> ConcurrentTables<TParameter, TResult>.Default.Get(@this);

		public static ITable<TParameter, TResult> ToTable<TParameter, TResult>(
			this ConditionalWeakTable<TParameter, TResult> @this) where TResult : class where TParameter : class
			=> ReferenceValueTables<TParameter, TResult>.Default.Get(@this);

		public static ITable<TParameter, TResult> ToTable<TParameter, TResult>(
			this ConditionalWeakTable<TParameter, Tuple<TResult>> @this) where TParameter : class where TResult : struct
			=> StructureValueTables<TParameter, TResult>.Default.Get(@this);

		public static ITable<TParameter, TResult> ToStandardTable<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this)
			=> @this.ToDelegate().ToStandardTable();

		public static ITable<TParameter, TResult> ToStandardTable<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this,
			IDictionary<TParameter, TResult> table)
			=> @this.ToDelegate().ToStandardTable(table);

		public static ITable<TParameter, TResult> ToStandardTable<TParameter, TResult>(this Func<TParameter, TResult> @this)
			=> @this.ToStandardTable(new Dictionary<TParameter, TResult>());

		public static ITable<TParameter, TResult> ToStandardTable<TParameter, TResult>(this Func<TParameter, TResult> @this,
		                                                                               IDictionary<TParameter, TResult> table)
			=> I<StandardTables<TParameter, TResult>>.Default.From(@this).Get(table);

		public static ITable<TParameter, TResult> ToConcurrentTable<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this)
			=> @this.ToDelegate().ToConcurrentTable();

		public static ITable<TParameter, TResult> ToConcurrentTable<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this,
			ConcurrentDictionary<TParameter, TResult> table)
			=> @this.ToDelegate().ToConcurrentTable(table);

		public static ITable<TParameter, TResult> ToConcurrentTable<TParameter, TResult>(this Func<TParameter, TResult> @this)
			=> @this.ToConcurrentTable(new ConcurrentDictionary<TParameter, TResult>());

		public static ITable<TParameter, TResult> ToConcurrentTable<TParameter, TResult>(this Func<TParameter, TResult> @this,
		                                                                                 ConcurrentDictionary<TParameter,
			                                                                                 TResult> table)
			=> I<ConcurrentTables<TParameter, TResult>>.Default.From(@this).Get(table);
	}
}
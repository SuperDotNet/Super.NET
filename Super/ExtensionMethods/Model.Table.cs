using Super.Model.Selection;
using Super.Model.Selection.Stores;
using Super.Reflection;
using Super.Runtime.Activation;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Super.ExtensionMethods
{
	partial class Model
	{
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
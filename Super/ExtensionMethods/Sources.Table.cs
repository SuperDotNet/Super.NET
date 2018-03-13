using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Super.Model.Sources;
using Super.Model.Sources.Tables;
using Super.Reflection;

namespace Super.ExtensionMethods
{
	partial class Sources
	{
		public static ITable<TParameter, TResult> ToStandardTable<TParameter, TResult>(
			this ISource<TParameter, TResult> @this)
			=> @this.ToDelegate().ToStandardTable();

		public static ITable<TParameter, TResult> ToStandardTable<TParameter, TResult>(
			this ISource<TParameter, TResult> @this,
			IDictionary<TParameter, TResult> table)
			=> @this.ToDelegate().ToStandardTable(table);

		public static ITable<TParameter, TResult> ToStandardTable<TParameter, TResult>(this Func<TParameter, TResult> @this)
			=> @this.ToStandardTable(new Dictionary<TParameter, TResult>());

		public static ITable<TParameter, TResult> ToStandardTable<TParameter, TResult>(this Func<TParameter, TResult> @this,
		                                                                               IDictionary<TParameter, TResult> table)
			=> I<StandardTables<TParameter, TResult>>.Default.Get(@this).Get(table);

		public static ITable<TParameter, TResult> ToConcurrentTable<TParameter, TResult>(
			this ISource<TParameter, TResult> @this)
			=> @this.ToDelegate().ToConcurrentTable();

		public static ITable<TParameter, TResult> ToConcurrentTable<TParameter, TResult>(
			this ISource<TParameter, TResult> @this,
			ConcurrentDictionary<TParameter, TResult> table)
			=> @this.ToDelegate().ToConcurrentTable(table);

		public static ITable<TParameter, TResult> ToConcurrentTable<TParameter, TResult>(this Func<TParameter, TResult> @this)
			=> @this.ToConcurrentTable(new ConcurrentDictionary<TParameter, TResult>());

		public static ITable<TParameter, TResult> ToConcurrentTable<TParameter, TResult>(this Func<TParameter, TResult> @this,
		                                                                                 ConcurrentDictionary<TParameter,
			                                                                                 TResult> table)
			=> I<ConcurrentTables<TParameter, TResult>>.Default.Get(@this).Get(table);
	}
}
using Super.Model.Selection;
using Super.Model.Selection.Adapters;
using Super.Model.Sequences;
using Super.Model.Sequences.Query;
using System;
using System.Collections.Generic;

namespace Super
{
	// ReSharper disable once MismatchedFileName
	public static partial class ExtensionMethods
	{
		public static Query<_, T> Select<_, T>(this Query<_, T> @this, Selection selection)
			=> @this.Skip(selection.Start).Take(selection.Length);

		public static Query<_, T> Append<_, T>(this Query<_, T> @this, ISequence<T> others)
			=> @this.Select(new Build.Concatenation<T>(others).Returned());

		public static Query<_, T> Union<_, T>(this Query<_, T> @this, ISequence<T> others)
			=> @this.Union(others, EqualityComparer<T>.Default);

		public static Query<_, T> Union<_, T>(this Query<_, T> @this, ISequence<T> others,
		                                      IEqualityComparer<T> comparer)
			=> @this.Select(new Build.Union<T>(others, comparer).Returned());

		public static Query<_, T> Intersect<_, T>(this Query<_, T> @this, ISequence<T> others)
			=> @this.Intersect(others, EqualityComparer<T>.Default);

		public static Query<_, T> Intersect<_, T>(this Query<_, T> @this, ISequence<T> others,
		                                          IEqualityComparer<T> comparer)
			=> @this.Select(new Build.Intersect<T>(others, comparer).Returned());

		public static Query<_, T> Distinct<_, T>(this Query<_, T> @this) => @this.Select(Build.Distinct<T>.Default);

		public static Query<_, T> Distinct<_, T>(this Query<_, T> @this, IEqualityComparer<T> comparer)
			=> @this.Select(new Build.Distinct<T>(comparer));

		public static Query<_, T> Skip<_, T>(this Query<_, T> @this, uint count) => @this.Select(new Skip(count));

		public static Query<_, T> Take<_, T>(this Query<_, T> @this, uint count) => @this.Select(new Take(count));

		public static Query<_, T> Where<_, T>(this Query<_, T> @this, ISelect<T, bool> where) => @this.Where(where.Get);

		public static Query<_, T> Where<_, T>(this Query<_, T> @this, Func<T, bool> where)
			=> @this.Select(new Build.Where<T>(where));

		public static ISelect<_, IArrayMap<TKey, T>> GroupMap<_, T, TKey>(
			this Query<_, T> @this, ISelect<T, TKey> select)
			=> @this.GroupMap(select.Get);

		public static ISelect<_, IArrayMap<TKey, T>> GroupMap<_, T, TKey>(this Query<_, T> @this, Func<T, TKey> select)
			=> @this.Select(new GroupMap<T, TKey>(select));

		public static ISelect<_, T> Only<_, T>(this Query<_, T> @this)
			=> @this.Select(Model.Sequences.Query.Only<T>.Default);

		public static ISelect<_, T> Only<_, T>(this Query<_, T> @this, Func<T, bool> where)
			=> @this.Select(new Only<T>(where));

		public static ISelect<_, T> First<_, T>(this Query<_, T> @this) => @this.Select(FirstOrDefault<T>.Default);

		public static ISelect<_, T> First<_, T>(this Query<_, T> @this, Func<T, bool> where)
			=> @this.Select(new FirstWhere<T>(where));

		public static ISelect<_, T> Single<_, T>(this Query<_, T> @this) => @this.Select(Single<T>.Default);

		public static ISelect<_, T> Single<_, T>(this Query<_, T> @this, Func<T, bool> where)
			=> @this.Select(new Single<T>(where));

		public static ISelect<_, int> Sum<_, T>(this Query<_, T> @this, Func<T, int> select)
			=> @this.Select(new Sum32<T>(select));

		public static ISelect<_, uint> Sum<_, T>(this Query<_, T> @this, Func<T, uint> select)
			=> @this.Select(new SumUnsigned32<T>(select));

		public static ISelect<_, long> Sum<_, T>(this Query<_, T> @this, Func<T, long> select)
			=> @this.Select(new Sum64<T>(select));

		public static ISelect<_, ulong> Sum<_, T>(this Query<_, T> @this, Func<T, ulong> select)
			=> @this.Select(new SumUnsigned64<T>(select));

		public static ISelect<_, float> Sum<_, T>(this Query<_, T> @this, Func<T, float> select)
			=> @this.Select(new SumSingle<T>(select));

		public static ISelect<_, double> Sum<_, T>(this Query<_, T> @this, Func<T, double> select)
			=> @this.Select(new SumDouble<T>(select));

		public static ISelect<_, decimal> Sum<_, T>(this Query<_, T> @this, Func<T, decimal> select)
			=> @this.Select(new SumDecimal<T>(select));
	}
}
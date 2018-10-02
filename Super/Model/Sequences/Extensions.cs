using Super.Model.Collections;
using Super.Model.Selection;
using System;
using System.Linq.Expressions;

namespace Super.Model.Sequences
{
	public static class Extensions
	{
		public static IQuery<_, T> Query<_, T>(this ISelect<_, T[]> @this) => new ArrayQuery<_, T>(@this);

		public static IQuery<_, T> Select<_, T>(this IQuery<_, T> @this, Collections.Selection selection)
			=> @this.Skip(selection.Start)
			        .Take(selection.Length);

		public static IQuery<_, T> Skip<_, T>(this IQuery<_, T> @this, uint skip) => @this.Get(new Skip(skip));

		public static IQuery<_, T> Take<_, T>(this IQuery<_, T> @this, uint take) => @this.Get(new Take(take));

		public static IQuery<_, T> WhereBy<_, T>(this IQuery<_, T> @this, Expression<Func<T, bool>> where)
			=> @this.Where(where.Compile());

		public static IQuery<_, T> Where<_, T>(this IQuery<_, T> @this, Func<T, bool> where)
			=> @this.Get(new Where<T>(where));

		public static ISelect<_, Array<T>> Result<_, T>(this IQuery<_, T> @this) => @this.Get().Array();

		/**/

		public static Session<T> Session<T>(this IStore<T> @this, uint amount)
			=> new Session<T>(@this.Get(amount), @this, amount);

		public static Session<T> Session<T>(this IStore<T> @this, T[] items) => new Session<T>(items, @this);
	}
}
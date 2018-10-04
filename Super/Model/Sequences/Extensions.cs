using Super.Model.Collections;
using Super.Model.Selection;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Super.Model.Sequences
{
	public static class Extensions
	{
		public static ISequence<_, T> Sequence<_, T>(this ISelect<_, T[]> @this) => new ArraySequence<_, T>(@this);

		public static ISequence<_, T> Sequence<_, T>(this ISelect<_, ICollection<T>> @this) => new CollectionSequence<_, T>(@this);

		public static ISequence<_, T> Select<_, T>(this ISequence<_, T> @this, Collections.Selection selection)
			=> @this.Skip(selection.Start)
			        .Take(selection.Length);

		public static ISequence<_, T> Skip<_, T>(this ISequence<_, T> @this, uint skip) => @this.Get(new Skip(skip));

		public static ISequence<_, T> Take<_, T>(this ISequence<_, T> @this, uint take) => @this.Get(new Take(take));

		public static ISequence<_, T> WhereBy<_, T>(this ISequence<_, T> @this, Expression<Func<T, bool>> where)
			=> @this.Where(where.Compile());

		public static ISequence<_, T> Where<_, T>(this ISequence<_, T> @this, Func<T, bool> where)
			=> @this.Get(new Where<T>(where));

		public static ISelect<_, Array<T>> Result<_, T>(this ISequence<_, T> @this) => @this.Get().Array();

		/**/

		public static Session<T> Session<T>(this IStore<T> @this, uint amount)
			=> new Session<T>(@this.Get(amount), @this, amount);

		public static Session<T> Session<T>(this IStore<T> @this, T[] items) => new Session<T>(items, @this);
	}
}
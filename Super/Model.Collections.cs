using Super.Model.Selection;
using Super.Model.Sequences;
using Super.Model.Sequences.Query;
using System;
using System.Collections.Generic;
using System.Linq;

// ReSharper disable TooManyArguments

namespace Super
{
	// ReSharper disable once MismatchedFileName

	public static partial class ExtensionMethods
	{
		public static IReadOnlyDictionary<TKey, Array<TValue>>
			ToDictionary<TKey, TValue>(this IEnumerable<IGrouping<TKey, TValue>> @this)
			=> @this.ToDictionary(x => x.Key, x => new Array<TValue>(x.ToArray()));

		public static ISelect<_, IReadOnlyDictionary<TKey, Array<T>>>
			Grouping<_, T, TKey>(this ISelect<_, Array<T>> @this, ISelect<T, TKey> select)
			=> @this.Grouping(select.Get);

		public static ISelect<_, IReadOnlyDictionary<TKey, Array<T>>>
			Grouping<_, T, TKey>(this ISelect<_, Array<T>> @this, Func<T, TKey> select)
			=> @this.Select(new Grouping<T, TKey>(select));
	}
}
using Super.Model.Selection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Super.Model.Sequences.Query
{
	public sealed class Lookup<TKey, TElement> : Model.Selection.Stores.Lookup<TKey, Array<TElement>>
	{
		public Lookup(IReadOnlyDictionary<TKey, Array<TElement>> store) : base(store, Array<TElement>.Empty) {}
	}

	sealed class Grouping<T, TKey> : ISelect<Array<T>, IReadOnlyDictionary<TKey, Array<T>>>
	{
		readonly Func<T, TKey> _select;

		public Grouping(Func<T, TKey> select) => _select = @select;

		public IReadOnlyDictionary<TKey, Array<T>> Get(Array<T> parameter)
			=> parameter.Reference().GroupBy(_select).ToDictionary();
	}
}

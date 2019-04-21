using Super.Compose;
using System.Collections.Generic;

namespace Super.Model.Sequences.Query
{
	public sealed class Lookup<TKey, TElement> : Model.Selection.Stores.Lookup<TKey, Array<TElement>>
	{
		public Lookup(IReadOnlyDictionary<TKey, Array<TElement>> store)
			: base(store, Start.A.Selection<TKey>().By.Returning(Array<TElement>.Empty).Get) {}
	}
}
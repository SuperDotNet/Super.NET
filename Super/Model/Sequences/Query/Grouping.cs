using Super.Model.Selection;
using System;
using System.Linq;

namespace Super.Model.Sequences.Query
{
	sealed class Grouping<T, TKey> : ISelect<Array<T>, IArrayMap<TKey, T>>
	{
		readonly Func<T, TKey> _select;

		public Grouping(Func<T, TKey> select) => _select = select;

		public IArrayMap<TKey, T> Get(Array<T> parameter)
			=> new ArrayMap<TKey, T>(parameter.Open().GroupBy(_select).ToDictionary().ToStore());
	}
}

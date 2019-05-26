using System;
using System.Linq;

namespace Super.Model.Sequences.Query
{
	sealed class GroupMap<T, TKey> : IReduce<T, IArrayMap<TKey, T>>
	{
		readonly Func<T, TKey> _select;

		public GroupMap(Func<T, TKey> select) => _select = select;

		public IArrayMap<TKey, T> Get(Store<T> parameter)
			=> new ArrayMap<TKey, T>(new ArrayView<T>(parameter.Instance, 0, parameter.Length)
			                         .ToArray() // TODO: Reduce allocations
			                         .GroupBy(_select)
			                         .ToDictionary(x => x.Key, x => x.ToArray().Result())
			                         .ToStore());
	}
}
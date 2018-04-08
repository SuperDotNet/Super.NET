using System.Collections.Concurrent;
using Super.Model.Specifications;

namespace Super.Model.Selection.Stores
{
	public sealed class ConcurrentDictionaryRemoveAdapter<TKey, TValue> : ISpecification<TKey>
	{
		readonly ConcurrentDictionary<TKey, TValue> _store;

		public ConcurrentDictionaryRemoveAdapter(ConcurrentDictionary<TKey, TValue> store) => _store = store;

		public bool IsSatisfiedBy(TKey parameter) => _store.TryRemove(parameter, out _);
	}
}
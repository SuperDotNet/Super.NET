using System.Collections.Concurrent;
using Super.Model.Selection.Conditions;

namespace Super.Model.Selection.Stores
{
	public sealed class ConcurrentDictionaryRemoveAdapter<TKey, TValue> : ICondition<TKey>
	{
		readonly ConcurrentDictionary<TKey, TValue> _store;

		public ConcurrentDictionaryRemoveAdapter(ConcurrentDictionary<TKey, TValue> store) => _store = store;

		public bool Get(TKey parameter) => _store.TryRemove(parameter, out _);
	}
}
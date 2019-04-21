using System.Runtime.CompilerServices;
using Super.Model.Selection.Conditions;

namespace Super.Model.Selection.Stores
{
	public sealed class ConditionalWeakTableContainsAdapter<TKey, TValue> : ICondition<TKey>
		where TKey : class where TValue : class
	{
		readonly ConditionalWeakTable<TKey, TValue> _store;

		public ConditionalWeakTableContainsAdapter(ConditionalWeakTable<TKey, TValue> store) => _store = store;

		public bool Get(TKey parameter) => _store.TryGetValue(parameter, out _);
	}
}
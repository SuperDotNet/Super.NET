using System.Runtime.CompilerServices;
using Super.Model.Specifications;

namespace Super.Model.Selection.Stores
{
	public sealed class ConditionalWeakTableContainsAdapter<TKey, TValue> : ISpecification<TKey>
		where TKey : class where TValue : class
	{
		readonly ConditionalWeakTable<TKey, TValue> _store;

		public ConditionalWeakTableContainsAdapter(ConditionalWeakTable<TKey, TValue> store) => _store = store;

		public bool IsSatisfiedBy(TKey parameter) => _store.TryGetValue(parameter, out _);
	}
}
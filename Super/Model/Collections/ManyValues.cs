using System.Collections.Generic;

namespace Super.Model.Collections
{
	public class ManyValues<TKey, TEnumerable, TValue> : Values<TKey, TEnumerable> where TEnumerable : IEnumerable<TValue>
	{
		public ManyValues(IDictionary<TKey, TEnumerable> dictionary) : base(dictionary) {}
	}
}
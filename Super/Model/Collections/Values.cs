using System.Collections.Generic;

namespace Super.Model.Collections
{
	public class Values<TKey, TValue> : ItemsBase<TValue>
	{
		readonly IDictionary<TKey, TValue> _dictionary;

		public Values(IDictionary<TKey, TValue> dictionary) => _dictionary = dictionary;

		public override IEnumerator<TValue> GetEnumerator()
			=> _dictionary.Values.GetEnumerator();
	}

	public class ManyValues<TKey, TEnumerable, TValue> : Values<TKey, TEnumerable> where TEnumerable : IEnumerable<TValue>
	{
		public ManyValues(IDictionary<TKey, TEnumerable> dictionary) : base(dictionary) {}
	}
}
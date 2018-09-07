using System.Collections.Generic;

namespace Super.Model.Collections
{
	public class Values<TKey, TValue> : Enumerable<TValue>
	{
		readonly IDictionary<TKey, TValue> _dictionary;

		public Values(IDictionary<TKey, TValue> dictionary) => _dictionary = dictionary;

		public override IEnumerator<TValue> GetEnumerator()
			=> _dictionary.Values.GetEnumerator();
	}
}
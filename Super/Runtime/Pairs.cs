using System.Collections.Generic;

namespace Super.Runtime
{
	public static class Pairs
	{
		public static KeyValuePair<TKey, TValue> Create<TKey, TValue>(TKey key, TValue value) =>
			new KeyValuePair<TKey, TValue>(key, value);
	}
}
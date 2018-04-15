using System.Collections;
using System.Collections.Generic;

namespace Super.Diagnostics.Logging
{
	sealed class Scalar : IScalar
	{
		readonly IReadOnlyDictionary<string, ScalarProperty> _properties;

		public Scalar(IReadOnlyDictionary<string, ScalarProperty> properties) => _properties = properties;

		public IEnumerator<KeyValuePair<string, ScalarProperty>> GetEnumerator() => _properties.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_properties).GetEnumerator();

		public int Count => _properties.Count;

		public bool ContainsKey(string key) => _properties.ContainsKey(key);

		public bool TryGetValue(string key, out ScalarProperty value) => _properties.TryGetValue(key, out value);

		public ScalarProperty this[string key] => _properties[key];

		public IEnumerable<string> Keys => _properties.Keys;

		public IEnumerable<ScalarProperty> Values => _properties.Values;
	}
}
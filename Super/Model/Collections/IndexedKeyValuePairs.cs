using System.Collections;
using System.Diagnostics;

namespace Super.Model.Collections
{
	/// <summary>
	/// ATTRIBUTION: https://github.com/mattmc3/dotmore
	/// </summary>
	[DebuggerDisplay("{Value}", Name = "[{Index}]: {Key}")]
	class IndexedKeyValuePairs
	{
		// ReSharper disable once TooManyDependencies
		public IndexedKeyValuePairs(IDictionary dictionary, int index, object key, object value)
		{
			Index      = index;
			Value      = value;
			Key        = key;
			Dictionary = dictionary;
		}

		public IDictionary Dictionary { get; }
		public int Index { get; }
		public object Key { get; }
		public object Value { get; }
	}
}
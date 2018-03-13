using System.Collections.Specialized;
using System.Diagnostics;

namespace Super.Model.Collections
{
	/// <summary>
	/// ATTRIBUTION: https://github.com/mattmc3/dotmore
	/// </summary>
	class OrderedDictionaryDebugView
	{
		readonly IOrderedDictionary _dict;

		public OrderedDictionaryDebugView(IOrderedDictionary dict) => _dict = dict;

		[DebuggerBrowsable(DebuggerBrowsableState.Collapsed)]
		public IndexedKeyValuePairs[] IndexedKeyValuePairs
		{
			get
			{
				var nkeys = new IndexedKeyValuePairs[_dict.Count];

				var i = 0;
				foreach (var key in _dict.Keys)
				{
					nkeys[i] =  new IndexedKeyValuePairs(_dict, i, key, _dict[key]);
					i        += 1;
				}

				return nkeys;
			}
		}
	}
}
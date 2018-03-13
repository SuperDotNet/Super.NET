using System.Collections.Generic;
using Super.Model.Commands;

namespace Super.Model.Sources.Tables
{
	public sealed class DictionaryAssignCommand<TKey, TValue> : ICommand<(TKey, TValue)>
	{
		readonly IDictionary<TKey, TValue> _store;

		public DictionaryAssignCommand(IDictionary<TKey, TValue> store) => _store = store;

		public void Execute((TKey, TValue) parameter) => _store[parameter.Item1] = parameter.Item2;
	}
}
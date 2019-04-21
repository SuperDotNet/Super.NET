using System;
using System.Collections.Generic;

namespace Super.Model.Selection.Stores
{
	public sealed class DictionaryAccessAdapter<TIn, TOut> : ISelect<TIn, TOut>
	{
		readonly Func<TIn, TOut>        _create;
		readonly IDictionary<TIn, TOut> _store;

		public DictionaryAccessAdapter(IDictionary<TIn, TOut> store, Func<TIn, TOut> create)
		{
			_store  = store;
			_create = create;
		}

		public TOut Get(TIn parameter)
		{
			if (_store.TryGetValue(parameter, out var existing))
			{
				return existing;
			}

			var result = _create(parameter);
			_store[parameter] = result;
			return result;
		}
	}
}
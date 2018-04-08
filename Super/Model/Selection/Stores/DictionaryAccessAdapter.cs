using System;
using System.Collections.Generic;

namespace Super.Model.Selection.Stores
{
	public sealed class DictionaryAccessAdapter<TParameter, TResult> : ISelect<TParameter, TResult>
	{
		readonly Func<TParameter, TResult>        _create;
		readonly IDictionary<TParameter, TResult> _store;

		public DictionaryAccessAdapter(IDictionary<TParameter, TResult> store, Func<TParameter, TResult> create)
		{
			_store  = store;
			_create = create;
		}

		public TResult Get(TParameter parameter)
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
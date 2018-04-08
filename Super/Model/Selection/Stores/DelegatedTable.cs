using System;
using System.Collections.Generic;
using Super.ExtensionMethods;

namespace Super.Model.Selection.Stores
{
	public class DelegatedTable<TParameter, TResult> : ITable<TParameter, TResult>
	{
		readonly Action<(TParameter, TResult)> _assign;
		readonly Func<TParameter, bool>        _contains;
		readonly Func<TParameter, TResult>     _get;
		readonly Func<TParameter, bool>        _remove;

		// ReSharper disable once TooManyDependencies
		public DelegatedTable(Func<TParameter, bool> contains, Action<(TParameter, TResult)> assign,
		                      Func<TParameter, TResult> get,
		                      Func<TParameter, bool> remove)
		{
			_contains = contains;
			_assign   = assign;
			_get      = get;
			_remove   = remove;
		}

		public bool IsSatisfiedBy(TParameter parameter) => _contains(parameter);

		public TResult Get(TParameter key) => _get(key);

		public bool Remove(TParameter key) => _remove(key);

		public void Execute(KeyValuePair<TParameter, TResult> parameter) => _assign(parameter.Key.Pair(parameter.Value));
	}
}
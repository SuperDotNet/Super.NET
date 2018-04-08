using System;
using System.Collections.Generic;
using Super.Runtime.Activation;

namespace Super.Model.Selection.Stores
{
	public class StandardTables<TParameter, TResult>
		: ISelect<IDictionary<TParameter, TResult>, ITable<TParameter, TResult>>, IActivateMarker<Func<TParameter, TResult>>
	{
		readonly Func<TParameter, TResult> _source;

		public StandardTables(Func<TParameter, TResult> source) => _source = source;

		public ITable<TParameter, TResult> Get(IDictionary<TParameter, TResult> parameter)
		{
			var get    = new DictionaryAccessAdapter<TParameter, TResult>(parameter, _source);
			var assign = new DictionaryAssignCommand<TParameter, TResult>(parameter);
			var result =
				new DelegatedTable<TParameter, TResult>(parameter.ContainsKey, assign.Execute, get.Get, parameter.Remove);
			return result;
		}
	}
}
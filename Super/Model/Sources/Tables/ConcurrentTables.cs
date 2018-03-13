using System;
using System.Collections.Concurrent;
using Super.Runtime.Activation;

namespace Super.Model.Sources.Tables
{
	public sealed class ConcurrentTables<TParameter, TResult>
		: ISource<ConcurrentDictionary<TParameter, TResult>, ITable<TParameter, TResult>>,
		  IActivateMarker<Func<TParameter, TResult>>
	{
		readonly Func<TParameter, TResult> _source;

		public ConcurrentTables(Func<TParameter, TResult> source) => _source = source;

		public ITable<TParameter, TResult> Get(ConcurrentDictionary<TParameter, TResult> parameter)
		{
			var get    = new TableAccessAdapter<TParameter, TResult>(parameter.GetOrAdd, _source);
			var remove = new ConcurrentDictionaryRemoveAdapter<TParameter, TResult>(parameter);
			var assign = new DictionaryAssignCommand<TParameter, TResult>(parameter);
			var result =
				new DelegatedTable<TParameter, TResult>(parameter.ContainsKey, assign.Execute, get.Get, remove.IsSatisfiedBy);
			return result;
		}
	}
}
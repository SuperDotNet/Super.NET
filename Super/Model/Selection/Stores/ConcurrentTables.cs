using Super.Runtime.Activation;
using System;
using System.Collections.Concurrent;

namespace Super.Model.Selection.Stores
{
	public sealed class ConcurrentTables<TParameter, TResult>
		: ISelect<ConcurrentDictionary<TParameter, TResult>, ITable<TParameter, TResult>>,
		  IActivateMarker<Func<TParameter, TResult>>
	{
		public static ConcurrentTables<TParameter, TResult> Default { get; } = new ConcurrentTables<TParameter, TResult>();

		ConcurrentTables() : this(Default<TParameter, TResult>.Instance.Get) {}

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
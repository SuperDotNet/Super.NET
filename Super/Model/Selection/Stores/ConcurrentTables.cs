using Super.Runtime.Activation;
using System;
using System.Collections.Concurrent;

namespace Super.Model.Selection.Stores
{
	public sealed class ConcurrentTables<TIn, TOut>
		: ISelect<ConcurrentDictionary<TIn, TOut>, ITable<TIn, TOut>>,
		  IActivateUsing<Func<TIn, TOut>>
	{
		public static ConcurrentTables<TIn, TOut> Default { get; } = new ConcurrentTables<TIn, TOut>();

		ConcurrentTables() : this(Default<TIn, TOut>.Instance.Get) {}

		readonly Func<TIn, TOut> _source;

		public ConcurrentTables(Func<TIn, TOut> source) => _source = source;

		public ITable<TIn, TOut> Get(ConcurrentDictionary<TIn, TOut> parameter)
		{
			var get    = new TableAccessAdapter<TIn, TOut>(parameter.GetOrAdd, _source);
			var remove = new ConcurrentDictionaryRemoveAdapter<TIn, TOut>(parameter);
			var assign = new DictionaryAssignCommand<TIn, TOut>(parameter);
			var result =
				new DelegatedTable<TIn, TOut>(parameter.ContainsKey, assign.Execute, get.Get, remove.Get);
			return result;
		}
	}
}
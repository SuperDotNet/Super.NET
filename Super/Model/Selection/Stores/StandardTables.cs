using Super.Runtime.Activation;
using System;
using System.Collections.Generic;

namespace Super.Model.Selection.Stores
{
	public class StandardTables<TIn, TOut>
		: ISelect<IDictionary<TIn, TOut>, ITable<TIn, TOut>>, IActivateUsing<Func<TIn, TOut>>
	{
		public static StandardTables<TIn, TOut> Default { get; } = new StandardTables<TIn, TOut>();

		StandardTables() : this(Default<TIn, TOut>.Instance.Get) {}

		readonly Func<TIn, TOut> _source;

		public StandardTables(Func<TIn, TOut> source) => _source = source;

		public ITable<TIn, TOut> Get(IDictionary<TIn, TOut> parameter)
		{
			var get    = new DictionaryAccessAdapter<TIn, TOut>(parameter, _source);
			var assign = new DictionaryAssignCommand<TIn, TOut>(parameter);
			var result =
				new DelegatedTable<TIn, TOut>(parameter.ContainsKey, assign.Execute, get.Get, parameter.Remove);
			return result;
		}
	}
}
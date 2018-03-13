using System.Collections.Generic;
using Super.Runtime;

namespace Super.Model.Sources.Tables
{
	public class CoercedTable<TFrom, TTo, TValue> : ITable<TTo, TValue>
	{
		readonly ISource<TTo, TFrom>   _coercer;
		readonly ITable<TFrom, TValue> _table;

		public CoercedTable(ITable<TFrom, TValue> table, ISource<TTo, TFrom> coercer)
		{
			_table   = table;
			_coercer = coercer;
		}

		public bool IsSatisfiedBy(TTo parameter) => _table.IsSatisfiedBy(_coercer.Get(parameter));

		public TValue Get(TTo parameter) => _table.Get(_coercer.Get(parameter));

		public void Execute(KeyValuePair<TTo, TValue> parameter)
		{
			_table.Execute(Pairs.Create(_coercer.Get(parameter.Key), parameter.Value));
		}

		public bool Remove(TTo key) => _table.Remove(_coercer.Get(key));
	}
}
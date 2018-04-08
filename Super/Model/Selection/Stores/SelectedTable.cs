using Super.Runtime;
using System.Collections.Generic;

namespace Super.Model.Selection.Stores
{
	public class SelectedTable<TFrom, TTo, TValue> : ITable<TTo, TValue>
	{
		readonly ISelect<TTo, TFrom>   _select;
		readonly ITable<TFrom, TValue> _table;

		public SelectedTable(ITable<TFrom, TValue> table, ISelect<TTo, TFrom> @select)
		{
			_table   = table;
			_select = @select;
		}

		public bool IsSatisfiedBy(TTo parameter) => _table.IsSatisfiedBy(_select.Get(parameter));

		public TValue Get(TTo parameter) => _table.Get(_select.Get(parameter));

		public void Execute(KeyValuePair<TTo, TValue> parameter)
		{
			_table.Execute(Pairs.Create(_select.Get(parameter.Key), parameter.Value));
		}

		public bool Remove(TTo key) => _table.Remove(_select.Get(key));
	}
}
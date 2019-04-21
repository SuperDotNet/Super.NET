using Super.Model.Selection.Conditions;
using Super.Runtime;

namespace Super.Model.Selection.Stores
{
	public class SelectedTable<TFrom, TTo, TValue> : SelectedConditional<TTo, TFrom, TValue>, ITable<TTo, TValue>
	{
		readonly ISelect<TTo, TFrom>       _select;
		readonly ITable<TFrom, TValue>     _table;

		public SelectedTable(ITable<TFrom, TValue> table, ISelect<TTo, TFrom> @select) : base(table, select.Get)
		{
			_table       = table;
			_select      = @select;
		}

		public void Execute(Pair<TTo, TValue> parameter)
		{
			_table.Execute(Pairs.Create(_select.Get(parameter.Key), parameter.Value));
		}

		public bool Remove(TTo key) => _table.Remove(_select.Get(key));
	}
}
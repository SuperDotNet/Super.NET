using Super.Model.Commands;
using Super.Runtime;
using System.Collections.Generic;

namespace Super.Model.Selection.Stores
{
	public class Table<TIn, TOut> : Lookup<TIn, TOut>, ITable<TIn, TOut>
	{
		readonly IDictionary<TIn, TOut> _store;

		public Table() : this(new Dictionary<TIn, TOut>()) {}

		public Table(IDictionary<TIn, TOut> store) : base(store) => _store = store;

		public bool Remove(TIn key) => _store.Remove(key);

		public void Execute(Pair<TIn, TOut> parameter) => _store[parameter.Key] = parameter.Value;
	}

	class RemoveCommand<TIn, TOut> : ICommand<TIn>
	{
		readonly ITable<TIn, TOut> _table;

		public RemoveCommand(ITable<TIn, TOut> table) => _table = table;

		public void Execute(TIn parameter)
		{
			_table.Remove(parameter);
		}
	}
}
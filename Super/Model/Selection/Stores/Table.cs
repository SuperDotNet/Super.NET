using Super.Model.Commands;
using System.Collections.Generic;

namespace Super.Model.Selection.Stores
{
	public class Table<TParameter, TResult> : ITable<TParameter, TResult>
	{
		readonly IDictionary<TParameter, TResult> _store;

		public Table() : this(new Dictionary<TParameter, TResult>()) {}

		public Table(IDictionary<TParameter, TResult> store) => _store = store;

		public bool IsSatisfiedBy(TParameter parameter) => _store.ContainsKey(parameter);

		public TResult Get(TParameter parameter) =>
			_store.TryGetValue(parameter, out var result) ? result : default;

		public bool Remove(TParameter key) => _store.Remove(key);

		public void Execute(KeyValuePair<TParameter, TResult> parameter) => _store[parameter.Key] = parameter.Value;
	}

	class RemoveCommand<TParameter, TResult> : ICommand<TParameter>
	{
		readonly ITable<TParameter, TResult> _table;

		public RemoveCommand(ITable<TParameter, TResult> table) => _table = table;

		public void Execute(TParameter parameter)
		{
			_table.Remove(parameter);
		}
	}
}
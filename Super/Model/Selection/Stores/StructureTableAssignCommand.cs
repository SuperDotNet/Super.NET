using System;
using Super.ExtensionMethods;
using Super.Model.Commands;

namespace Super.Model.Selection.Stores
{
	public sealed class StructureTableAssignCommand<TKey, TValue> : ICommand<(TKey, TValue)>
	{
		readonly ICommand<(TKey, Tuple<TValue>)> _store;

		public StructureTableAssignCommand(ICommand<(TKey, Tuple<TValue>)> store) => _store = store;

		public void Execute((TKey, TValue) parameter) => _store.Execute(parameter.Item1, Tuple.Create(parameter.Item2));
	}
}
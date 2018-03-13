using System.Runtime.CompilerServices;
using Super.Model.Commands;

namespace Super.Model.Sources.Tables
{
	public sealed class ConditionalWeakTableAssignCommand<TKey, TValue> : ICommand<(TKey, TValue)>
		where TKey : class where TValue : class
	{
		readonly ConditionalWeakTable<TKey, TValue> _store;

		public ConditionalWeakTableAssignCommand(ConditionalWeakTable<TKey, TValue> store) => _store = store;

		public void Execute((TKey, TValue) parameter)
		{
			_store.Remove(parameter.Item1);
			_store.Add(parameter.Item1, parameter.Item2);
		}
	}
}
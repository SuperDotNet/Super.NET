using System;

namespace Super.Model.Selection.Stores
{
	public sealed class StructureTableAccessAdapter<TKey, TValue> : ISelect<TKey, TValue>
	{
		readonly ISelect<TKey, Tuple<TValue>> _select;

		public StructureTableAccessAdapter(ISelect<TKey, Tuple<TValue>> @select) => _select = @select;

		public TValue Get(TKey parameter) => _select.Get(parameter)
		                                            .Item1;
	}
}
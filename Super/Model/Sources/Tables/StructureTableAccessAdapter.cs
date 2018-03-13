using System;

namespace Super.Model.Sources.Tables
{
	public sealed class StructureTableAccessAdapter<TKey, TValue> : ISource<TKey, TValue>
	{
		readonly ISource<TKey, Tuple<TValue>> _source;

		public StructureTableAccessAdapter(ISource<TKey, Tuple<TValue>> source) => _source = source;

		public TValue Get(TKey parameter) => _source.Get(parameter)
		                                            .Item1;
	}
}
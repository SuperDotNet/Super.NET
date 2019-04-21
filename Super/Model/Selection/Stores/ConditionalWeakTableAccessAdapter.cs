using System.Runtime.CompilerServices;

namespace Super.Model.Selection.Stores
{
	public sealed class ConditionalWeakTableAccessAdapter<TIn, TOut> : ISelect<TIn, TOut>
		where TIn : class where TOut : class
	{
		readonly ConditionalWeakTable<TIn, TOut>.CreateValueCallback _callback;
		readonly ConditionalWeakTable<TIn, TOut>                     _table;

		public ConditionalWeakTableAccessAdapter(ConditionalWeakTable<TIn, TOut> table,
		                                         ConditionalWeakTable<TIn, TOut>.CreateValueCallback callback)
		{
			_table    = table;
			_callback = callback;
		}

		public TOut Get(TIn parameter) => _table.GetValue(parameter, _callback);
	}
}
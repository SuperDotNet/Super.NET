using System.Runtime.CompilerServices;

namespace Super.Model.Selection.Stores
{
	public sealed class ConditionalWeakTableAccessAdapter<TParameter, TResult> : ISelect<TParameter, TResult>
		where TParameter : class where TResult : class
	{
		readonly ConditionalWeakTable<TParameter, TResult>.CreateValueCallback _callback;
		readonly ConditionalWeakTable<TParameter, TResult>                     _table;

		public ConditionalWeakTableAccessAdapter(ConditionalWeakTable<TParameter, TResult> table,
		                                         ConditionalWeakTable<TParameter, TResult>.CreateValueCallback callback)
		{
			_table    = table;
			_callback = callback;
		}

		public TResult Get(TParameter parameter) => _table.GetValue(parameter, _callback);
	}
}
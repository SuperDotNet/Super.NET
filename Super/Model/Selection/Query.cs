using System.Collections;
using System.Collections.Generic;

namespace Super.Model.Selection
{
	public class Query<TParameter, TResult> : IQuery<TParameter, TResult>
	{
		readonly IEnumerable<TResult>         _items;
		readonly ISelect<TParameter, TResult> _select;

		public Query(ISelect<TParameter, TResult> @select, IEnumerable<TResult> items)
		{
			_select = @select;
			_items  = items;
		}

		public TResult Get(TParameter parameter) => _select.Get(parameter);

		public IEnumerator<TResult> GetEnumerator() => _items.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
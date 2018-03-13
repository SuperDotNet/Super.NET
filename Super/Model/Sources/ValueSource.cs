using System.Collections;
using System.Collections.Generic;

namespace Super.Model.Sources
{
	public class ValueSource<TParameter, TResult> : IValueSource<TParameter, TResult>
	{
		readonly IEnumerable<TResult>         _items;
		readonly ISource<TParameter, TResult> _source;

		public ValueSource(ISource<TParameter, TResult> source, IEnumerable<TResult> items)
		{
			_source = source;
			_items  = items;
		}

		public TResult Get(TParameter parameter) => _source.Get(parameter);

		public IEnumerator<TResult> GetEnumerator() => _items.GetEnumerator();

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
using Super.Model.Selection;
using System;

namespace Super.Model.Collections
{
	sealed class ArrayResult<_, T> : ISelect<_, T[]>
	{
		readonly ISelect<_, ArrayView<T>>           _view;
		readonly IEnhancedSelect<ArrayView<T>, T[]> _result;
		readonly Action<T[]>                        _return;

		public ArrayResult(ISelect<_, ArrayView<T>> view, IEnhancedSelect<ArrayView<T>, T[]> result,
		                   Action<T[]> @return = null)
		{
			_view   = view;
			_result = result;
			_return = @return;
		}

		public T[] Get(_ parameter)
		{
			var view = _view.Get(parameter);

			using (new Session<T>(view.Array, _return))
			{
				return _result.Get(in view);
			}
		}
	}
}
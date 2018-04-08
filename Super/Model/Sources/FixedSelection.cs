using System;
using Super.Model.Selection;

namespace Super.Model.Sources
{
	public class FixedSelection<TParameter, TResult> : ISource<TResult>
	{
		readonly TParameter                _parameter;
		readonly Func<TParameter, TResult> _source;

		public FixedSelection(ISelect<TParameter, TResult> @select, TParameter parameter)
			: this(@select.Get, parameter) {}

		public FixedSelection(Func<TParameter, TResult> source, TParameter parameter)
		{
			_source    = source;
			_parameter = parameter;
		}

		public TResult Get() => _source(_parameter);
	}
}
using System;

namespace Super.Model.Sources
{
	class SelectedParameterSource<TFrom, TTo, TResult> : ISource<TTo, TResult>
	{
		readonly Func<TTo, TFrom>     _select;
		readonly Func<TFrom, TResult> _source;

		public SelectedParameterSource(Func<TFrom, TResult> source, Func<TTo, TFrom> @select)
		{
			_select = @select;
			_source  = source;
		}

		public TResult Get(TTo parameter) => _source(_select(parameter));
	}
}
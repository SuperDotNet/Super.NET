using System;

namespace Super.Model.Selection
{
	sealed class Parameter<TFrom, TTo, TResult> : ISelect<TTo, TResult>
	{
		readonly Func<TTo, TFrom>     _select;
		readonly Func<TFrom, TResult> _source;

		public Parameter(Func<TFrom, TResult> source, Func<TTo, TFrom> @select)
		{
			_select = @select;
			_source  = source;
		}

		public TResult Get(TTo parameter) => _source(_select(parameter));
	}
}
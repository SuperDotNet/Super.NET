using System;

namespace Super.Model.Selection
{
	class Parameter<TFrom, TTo, TResult> : ISelect<TFrom, TResult>
	{
		readonly Func<TFrom, TTo>     _select;
		readonly Func<TTo, TResult> _source;

		public Parameter(Func<TTo, TResult> source, Func<TFrom, TTo> @select)
		{
			_select = @select;
			_source  = source;
		}

		public TResult Get(TFrom parameter) => _source(_select(parameter));
	}
}
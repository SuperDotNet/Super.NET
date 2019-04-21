using System;

namespace Super.Model.Selection
{
	class Parameter<TFrom, TTo, TOut> : ISelect<TFrom, TOut>
	{
		readonly Func<TFrom, TTo>     _select;
		readonly Func<TTo, TOut> _source;

		public Parameter(Func<TTo, TOut> source, Func<TFrom, TTo> @select)
		{
			_select = @select;
			_source  = source;
		}

		public TOut Get(TFrom parameter) => _source(_select(parameter));
	}
}
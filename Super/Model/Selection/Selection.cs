using System;

namespace Super.Model.Selection
{
	class Selection<TIn, TFrom, TTo> : ISelect<TIn, TTo>
	{
		readonly Func<TFrom, TTo> _select;
		readonly Func<TIn, TFrom> _source;

		public Selection(Func<TIn, TFrom> source, Func<TFrom, TTo> @select)
		{
			_select = @select;
			_source = source;
		}

		public TTo Get(TIn parameter) => _select(_source(parameter));
	}
}
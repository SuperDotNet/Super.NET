using System;

namespace Super.Model.Selection
{
	sealed class Selection<TIn, TFrom, TTo> : ISelect<TIn, TTo>
	{
		readonly Func<TFrom, TTo> _to;
		readonly Func<TIn, TFrom> _from;

		public Selection(Func<TIn, TFrom> from, Func<TFrom, TTo> to)
		{
			_from = from;
			_to   = to;
		}

		public TTo Get(TIn parameter) => _to(_from(parameter));
	}
}
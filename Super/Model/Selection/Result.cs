using System;

namespace Super.Model.Selection
{
	sealed class Result<TParameter, TFrom, TTo> : ISelect<TParameter, TTo>
	{
		readonly Func<TFrom, TTo>        _select;
		readonly Func<TParameter, TFrom> _source;

		public Result(Func<TParameter, TFrom> source, Func<TFrom, TTo> @select)
		{
			_select = @select;
			_source  = source;
		}

		public TTo Get(TParameter parameter) => _select(_source(parameter));
	}
}
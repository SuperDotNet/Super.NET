using System;

namespace Super.Model.Sources
{
	sealed class SelectedResult<TParameter, TFrom, TTo> : ISource<TParameter, TTo>
	{
		readonly Func<TFrom, TTo>        _coercer;
		readonly Func<TParameter, TFrom> _source;

		public SelectedResult(Func<TParameter, TFrom> source, Func<TFrom, TTo> coercer)
		{
			_coercer = coercer;
			_source  = source;
		}

		public TTo Get(TParameter parameter) => _coercer(_source(parameter));
	}
}
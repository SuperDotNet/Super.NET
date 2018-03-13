using System;

namespace Super.Model.Sources
{
	sealed class CoercedResult<TParameter, TResult, TTo> : ISource<TParameter, TTo>
	{
		readonly Func<TResult, TTo>        _coercer;
		readonly Func<TParameter, TResult> _source;

		public CoercedResult(Func<TParameter, TResult> source, Func<TResult, TTo> coercer)
		{
			_coercer = coercer;
			_source  = source;
		}

		public TTo Get(TParameter parameter) => _coercer(_source(parameter));
	}
}
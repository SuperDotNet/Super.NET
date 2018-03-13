using System;

namespace Super.Model.Sources
{
	class CoercedParameter<TFrom, TTo, TResult> : ISource<TTo, TResult>
	{
		readonly Func<TTo, TFrom>     _coercer;
		readonly Func<TFrom, TResult> _source;

		public CoercedParameter(Func<TFrom, TResult> source, Func<TTo, TFrom> coercer)
		{
			_coercer = coercer;
			_source  = source;
		}

		public TResult Get(TTo parameter) => _source(_coercer(parameter));
	}
}
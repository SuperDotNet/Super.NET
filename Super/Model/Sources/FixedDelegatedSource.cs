using System;

namespace Super.Model.Sources
{
	class FixedDelegatedSource<TParameter, TResult> : ISource<TParameter, TResult>
	{
		readonly Func<TResult> _source;

		public FixedDelegatedSource(Func<TResult> source) => _source = source;

		public TResult Get(TParameter _) => _source();
	}
}
using System;

namespace Super.Model.Selection
{
	class DelegatedResult<TParameter, TResult> : ISelect<TParameter, TResult>
	{
		readonly Func<TResult> _result;

		public DelegatedResult(Func<TResult> source) => _result = source;

		public TResult Get(TParameter _) => _result();
	}
}
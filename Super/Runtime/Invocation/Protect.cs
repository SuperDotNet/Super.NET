using Super.Model.Selection;
using Super.Runtime.Activation;
using System;

namespace Super.Runtime.Invocation
{
	sealed class Protect<TParameter, TResult> : ISelect<TParameter, TResult>, IActivateMarker<ISelect<TParameter, TResult>>
	{
		readonly Func<TParameter, TResult> _source;

		public Protect(ISelect<TParameter, TResult> @select) : this(@select.Get) {}

		public Protect(Func<TParameter, TResult> source) => _source = source;

		public TResult Get(TParameter parameter)
		{
			lock (_source)
			{
				return _source(parameter);
			}
		}
	}
}
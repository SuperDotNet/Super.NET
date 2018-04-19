using System;
using Super.Model.Selection;
using Super.Runtime.Activation;

namespace Super.Runtime.Invocation {
	sealed class Stripe<TParameter, TResult> : ISelect<TParameter, TResult>, IActivateMarker<Func<TParameter, TResult>>
	{
		readonly static Func<TParameter, object> Lock = Locks<TParameter>.Default.ToDelegate();

		readonly Func<TParameter, object>  _lock;
		readonly Func<TParameter, TResult> _source;

		public Stripe(Func<TParameter, TResult> source) : this(Lock, source) {}

		public Stripe(Func<TParameter, object> @lock, Func<TParameter, TResult> source)
		{
			_lock   = @lock;
			_source = source;
		}

		public TResult Get(TParameter parameter)
		{
			lock (_lock(parameter))
			{
				return _source(parameter);
			}
		}
	}
}
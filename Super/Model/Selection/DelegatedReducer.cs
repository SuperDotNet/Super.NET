using Super.Runtime.Activation;
using System;

namespace Super.Model.Selection
{
	sealed class DelegatedReducer<TParameter, TResult> : ISelect<ISelect<TParameter, TResult>, TResult>, IActivateMarker<Func<TParameter>>
	{
		public static DelegatedReducer<TParameter, TResult> Default { get; }
			= new DelegatedReducer<TParameter, TResult>();

		DelegatedReducer() : this(New<TParameter>.Default.Get) {}

		readonly Func<TParameter> _parameter;

		public DelegatedReducer(Func<TParameter> parameter) => _parameter = parameter;

		public TResult Get(ISelect<TParameter, TResult> parameter) => parameter.Get(_parameter());
	}
}
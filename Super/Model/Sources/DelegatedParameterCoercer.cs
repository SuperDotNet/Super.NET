using Super.ExtensionMethods;
using Super.Runtime.Activation;
using System;

namespace Super.Model.Sources
{
	sealed class DelegatedParameterCoercer<TParameter, TResult> : ISource<ISource<TParameter, TResult>, TResult>, IActivateMarker<Func<TParameter>>
	{
		public static DelegatedParameterCoercer<TParameter, TResult> Default { get; } =
			new DelegatedParameterCoercer<TParameter, TResult>();

		DelegatedParameterCoercer() : this(Activation<TParameter>.Default.ToDelegate()) {}

		readonly Func<TParameter> _parameter;

		public DelegatedParameterCoercer(Func<TParameter> parameter) => _parameter = parameter;

		public TResult Get(ISource<TParameter, TResult> parameter) => parameter.Get(_parameter());
	}
}
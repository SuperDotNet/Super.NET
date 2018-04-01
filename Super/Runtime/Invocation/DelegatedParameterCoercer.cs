using Super.ExtensionMethods;
using Super.Model.Sources;
using Super.Runtime.Activation;
using System;

namespace Super.Runtime.Invocation
{
	sealed class DelegatedParameterCoercer<TParameter, TResult> : ISource<ISource<TParameter, TResult>, TResult>
	{
		public static DelegatedParameterCoercer<TParameter, TResult> Default { get; } =
			new DelegatedParameterCoercer<TParameter, TResult>();

		DelegatedParameterCoercer() : this(New<TParameter>.Default.ToDelegate()) {}

		readonly Func<TParameter> _parameter;

		public DelegatedParameterCoercer(Func<TParameter> parameter) => _parameter = parameter;

		public TResult Get(ISource<TParameter, TResult> parameter) => parameter.Get(_parameter());
	}
}
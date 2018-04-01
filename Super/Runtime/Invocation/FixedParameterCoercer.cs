using Super.Model.Sources;

namespace Super.Runtime.Invocation
{
	sealed class FixedParameterCoercer<TParameter, TResult> : ISource<ISource<TParameter, TResult>, TResult>
	{
		readonly TParameter _parameter;

		public FixedParameterCoercer(TParameter parameter) => _parameter = parameter;

		public TResult Get(ISource<TParameter, TResult> parameter) => parameter.Get(_parameter);
	}
}
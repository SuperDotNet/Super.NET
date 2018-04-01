using Super.Model.Instances;
using System;

namespace Super.Model.Sources
{
	public class FixedParameterSource<TParameter, TResult> : IInstance<TResult>
	{
		readonly TParameter                _parameter;
		readonly Func<TParameter, TResult> _source;

		public FixedParameterSource(ISource<TParameter, TResult> source, TParameter parameter)
			: this(source.Get, parameter) {}

		public FixedParameterSource(Func<TParameter, TResult> source, TParameter parameter)
		{
			_source    = source;
			_parameter = parameter;
		}

		public TResult Get() => _source(_parameter);
	}
}
using System;
using Super.Model.Sources;

namespace Super.Runtime.Invocation
{
	public class Invocation0<T1, T2, TResult> : ISource<T1, TResult>
	{
		readonly Func<T1, T2, TResult> _delegate;
		readonly T2                    _parameter;

		public Invocation0(Func<T1, T2, TResult> @delegate, T2 parameter)
		{
			_delegate  = @delegate;
			_parameter = parameter;
		}

		public TResult Get(T1 parameter) => _delegate(parameter, _parameter);
	}
}
using System;
using Super.Model.Selection;

namespace Super.Runtime.Invocation
{
	class Invocation1<T1, T2, TResult> : ISelect<T2, TResult>
	{
		readonly Func<T1, T2, TResult> _delegate;
		readonly T1                    _parameter;

		public Invocation1(Func<T1, T2, TResult> @delegate, T1 parameter)
		{
			_delegate  = @delegate;
			_parameter = parameter;
		}

		public TResult Get(T2 parameter) => _delegate(_parameter, parameter);
	}
}
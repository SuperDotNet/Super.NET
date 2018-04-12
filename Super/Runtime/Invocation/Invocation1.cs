using Super.Model.Selection;
using System;

namespace Super.Runtime.Invocation
{
	public class Invocation1<T1, T2, TResult> : ISelect<T2, TResult>
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
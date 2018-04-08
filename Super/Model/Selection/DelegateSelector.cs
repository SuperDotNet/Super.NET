using Super.ExtensionMethods;
using System;

namespace Super.Model.Selection
{
	sealed class DelegateSelector<TParameter, TResult> : ISelect<ISelect<TParameter, TResult>, Func<TParameter, TResult>>
	{
		public static DelegateSelector<TParameter, TResult> Default { get; } = new DelegateSelector<TParameter, TResult>();

		DelegateSelector() {}

		public Func<TParameter, TResult> Get(ISelect<TParameter, TResult> parameter) => parameter.ToDelegate();
	}
}

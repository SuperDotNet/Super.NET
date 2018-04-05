using Super.ExtensionMethods;
using System;

namespace Super.Model.Sources
{
	sealed class DelegateCoercer<TParameter, TResult> : ISource<ISource<TParameter, TResult>, Func<TParameter, TResult>>
	{
		public static DelegateCoercer<TParameter, TResult> Default { get; } = new DelegateCoercer<TParameter, TResult>();

		DelegateCoercer() {}

		public Func<TParameter, TResult> Get(ISource<TParameter, TResult> parameter) => parameter.ToDelegate();
	}
}

using System;
using Super.ExtensionMethods;
using Super.Model.Selection;

namespace Super.Model.Sources
{
	sealed class DelegateSelector<T> : ISelect<ISource<T>, Func<T>>
	{
		public static ISelect<ISource<T>, Func<T>> Default { get; } = new DelegateSelector<T>();

		DelegateSelector() {}

		public Func<T> Get(ISource<T> parameter) => parameter.ToDelegate();
	}

	/*sealed class DelegateCoercer<TParameter, TResult> : ISource<IInstance<ISource<TParameter, TResult>>, IInstance<Func<TParameter, TResult>>>
	{
		public static DelegateCoercer<TParameter, TResult> Default { get; } = new DelegateCoercer<TParameter, TResult>();

		DelegateCoercer() {}

		public IInstance<Func<TParameter, TResult>> Get(IInstance<ISource<TParameter, TResult>> parameter)
			=> parameter.Adapt(Sources.DelegateCoercer<TParameter, TResult>.Default);
	}*/
}

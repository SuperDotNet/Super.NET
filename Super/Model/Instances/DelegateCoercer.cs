using Super.ExtensionMethods;
using Super.Model.Sources;
using System;

namespace Super.Model.Instances
{
	sealed class DelegateCoercer<T> : ISource<IInstance<T>, Func<T>>
	{
		public static ISource<IInstance<T>, Func<T>> Default { get; } = new DelegateCoercer<T>();

		DelegateCoercer() {}

		public Func<T> Get(IInstance<T> parameter) => parameter.ToDelegate();
	}

	/*sealed class DelegateCoercer<TParameter, TResult> : ISource<IInstance<ISource<TParameter, TResult>>, IInstance<Func<TParameter, TResult>>>
	{
		public static DelegateCoercer<TParameter, TResult> Default { get; } = new DelegateCoercer<TParameter, TResult>();

		DelegateCoercer() {}

		public IInstance<Func<TParameter, TResult>> Get(IInstance<ISource<TParameter, TResult>> parameter)
			=> parameter.Adapt(Sources.DelegateCoercer<TParameter, TResult>.Default);
	}*/
}

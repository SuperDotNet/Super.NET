using System;
using Super.Model.Instances;

namespace Super.Model.Sources.Alterations
{
	public sealed class SingletonDelegateCoercer<T> : IAlteration<Func<T>>
	{
		public static SingletonDelegateCoercer<T> Default { get; } = new SingletonDelegateCoercer<T>();

		SingletonDelegateCoercer() {}

		public Func<T> Get(Func<T> parameter) => new DeferredSingleton<T>(parameter).Get;
	}
}
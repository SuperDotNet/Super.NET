using Super.Model.Sources;
using System;

namespace Super.Model.Selection.Alterations
{
	public sealed class SingletonDelegateSelector<T> : IAlteration<Func<T>>
	{
		public static SingletonDelegateSelector<T> Default { get; } = new SingletonDelegateSelector<T>();

		SingletonDelegateSelector() {}

		public Func<T> Get(Func<T> parameter) => new DeferredSingleton<T>(parameter).Get;
	}
}
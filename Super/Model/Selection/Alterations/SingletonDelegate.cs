using Super.Model.Results;
using System;

namespace Super.Model.Selection.Alterations
{
	public sealed class SingletonDelegate<T> : IAlteration<Func<T>>
	{
		public static SingletonDelegate<T> Default { get; } = new SingletonDelegate<T>();

		SingletonDelegate() {}

		public Func<T> Get(Func<T> parameter) => new DeferredSingleton<T>(parameter).Get;
	}
}
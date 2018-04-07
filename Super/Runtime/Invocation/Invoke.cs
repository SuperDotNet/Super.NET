using Super.Model.Sources;
using System;

namespace Super.Runtime.Invocation
{
	sealed class Invoke<T> : ISource<Func<T>, T>
	{
		public static Invoke<T> Default { get; } = new Invoke<T>();

		Invoke() {}

		public T Get(Func<T> parameter) => parameter();
	}
}
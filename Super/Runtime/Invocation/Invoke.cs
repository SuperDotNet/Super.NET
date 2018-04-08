using System;
using Super.Model.Selection;

namespace Super.Runtime.Invocation
{
	sealed class Invoke<T> : ISelect<Func<T>, T>
	{
		public static Invoke<T> Default { get; } = new Invoke<T>();

		Invoke() {}

		public T Get(Func<T> parameter) => parameter();
	}
}
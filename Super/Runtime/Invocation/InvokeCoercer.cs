using Super.Model.Sources;
using System;

namespace Super.Runtime.Invocation
{
	sealed class InvokeCoercer<T> : ISource<Func<T>, T>
	{
		public static InvokeCoercer<T> Default { get; } = new InvokeCoercer<T>();

		InvokeCoercer() {}

		public T Get(Func<T> parameter) => parameter();
	}
}
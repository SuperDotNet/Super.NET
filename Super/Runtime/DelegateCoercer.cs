using System;
using Super.Model.Sources;

namespace Super.Runtime
{
	sealed class DelegateCoercer<T> : ISource<Func<T>, T>
	{
		public static DelegateCoercer<T> Default { get; } = new DelegateCoercer<T>();

		DelegateCoercer() {}

		public T Get(Func<T> parameter) => parameter();
	}
}
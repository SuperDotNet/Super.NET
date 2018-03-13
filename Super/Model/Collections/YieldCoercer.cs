using System.Collections.Generic;
using Super.ExtensionMethods;
using Super.Model.Sources;

namespace Super.Model.Collections
{
	public sealed class YieldCoercer<T> : ISource<T, IEnumerable<T>>
	{
		public static YieldCoercer<T> Default { get; } = new YieldCoercer<T>();

		YieldCoercer() {}

		public IEnumerable<T> Get(T parameter) => parameter.Yield();
	}
}
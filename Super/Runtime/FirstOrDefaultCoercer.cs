using System.Collections.Generic;
using System.Linq;
using Super.Model.Sources;

namespace Super.Runtime
{
	public sealed class FirstOrDefaultCoercer<T> : ISource<IEnumerable<T>, T>
	{
		public static FirstOrDefaultCoercer<T> Default { get; } = new FirstOrDefaultCoercer<T>();

		FirstOrDefaultCoercer() {}

		public T Get(IEnumerable<T> parameter) => parameter.FirstOrDefault();
	}
}
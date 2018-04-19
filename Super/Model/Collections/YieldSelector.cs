using Super.Model.Selection;
using System.Collections.Generic;

namespace Super.Model.Collections
{
	public sealed class YieldSelector<T> : ISelect<T, IEnumerable<T>>
	{
		public static YieldSelector<T> Default { get; } = new YieldSelector<T>();

		YieldSelector() {}

		public IEnumerable<T> Get(T parameter) => parameter.Yield();
	}
}
using Super.Model.Selection;
using System.Collections.Generic;
using System.Linq;

namespace Super.Model.Collections
{
	public sealed class FirstOrDefaultSelector<T> : Delegated<IEnumerable<T>, T>
	{
		public static FirstOrDefaultSelector<T> Default { get; } = new FirstOrDefaultSelector<T>();

		FirstOrDefaultSelector() : base(enumerable => enumerable.FirstOrDefault()) {}
	}
}
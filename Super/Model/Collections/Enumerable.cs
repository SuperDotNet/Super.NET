using System.Collections.Generic;

namespace Super.Model.Collections
{
	public class Enumerable<T> : ItemsBase<T>
	{
		readonly IEnumerable<T> _enumerable;

		public Enumerable(IEnumerable<T> enumerable) => _enumerable = enumerable;

		public sealed override IEnumerator<T> GetEnumerator() => _enumerable.GetEnumerator();
	}
}
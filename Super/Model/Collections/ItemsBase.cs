using System.Collections;
using System.Collections.Generic;

namespace Super.Model.Collections
{
	public abstract class ItemsBase<T> : IEnumerable<T>
	{
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public abstract IEnumerator<T> GetEnumerator();
	}
}
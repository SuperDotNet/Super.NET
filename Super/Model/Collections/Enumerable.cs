using System.Collections;
using System.Collections.Generic;

namespace Super.Model.Collections
{
	public abstract class Enumerable<T> : IEnumerable<T>
	{
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		public abstract IEnumerator<T> GetEnumerator();
	}
}
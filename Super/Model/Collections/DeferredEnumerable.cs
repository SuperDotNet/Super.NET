using System;
using System.Collections.Generic;

namespace Super.Model.Collections
{
	class DeferredEnumerable<T> : ItemsBase<T>
	{
		readonly Func<IEnumerable<T>> _enumerable;

		public DeferredEnumerable(Func<IEnumerable<T>> enumerable) => _enumerable = enumerable;

		public override IEnumerator<T> GetEnumerator() => _enumerable.Invoke()
		                                                             .GetEnumerator();
	}
}
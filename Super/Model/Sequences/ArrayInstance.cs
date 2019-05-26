using System.Collections.Generic;
using Super.Model.Results;

namespace Super.Model.Sequences
{
	public class ArrayInstance<T> : Instance<Array<T>>, IArray<T>
	{
		public ArrayInstance(IEnumerable<T> enumerable) : this(enumerable.Open()) {}

		public ArrayInstance(params T[] instance) : base(instance) {}
	}
}
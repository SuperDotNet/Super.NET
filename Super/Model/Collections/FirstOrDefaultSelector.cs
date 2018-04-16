using Super.Model.Selection;
using System.Collections.Generic;
using System.Linq;

namespace Super.Model.Collections
{
	public sealed class FirstOrDefaultSelector<T> : Select<IEnumerable<T>, T>
	{
		public static FirstOrDefaultSelector<T> Default { get; } = new FirstOrDefaultSelector<T>();

		FirstOrDefaultSelector() : base(enumerable => enumerable.FirstOrDefault()) {}
	}

	public sealed class SingleSelector<T> : Select<IEnumerable<T>, T>
	{
		public static SingleSelector<T> Default { get; } = new SingleSelector<T>();

		SingleSelector() : base(x => x.Single()) {}
	}
}
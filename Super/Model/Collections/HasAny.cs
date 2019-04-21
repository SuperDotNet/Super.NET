using System.Collections;
using System.Collections.Generic;
using Super.Model.Selection.Conditions;

namespace Super.Model.Collections
{
	public sealed class HasAny : DelegatedCondition<ICollection>
	{
		public static HasAny Default { get; } = new HasAny();

		HasAny() : base(x => x.Count > 0) {}
	}

	public sealed class HasAny<T> : DelegatedCondition<ICollection<T>>
	{
		public static HasAny<T> Default { get; } = new HasAny<T>();

		HasAny() : base(x => x.Count > 0) {}
	}
}
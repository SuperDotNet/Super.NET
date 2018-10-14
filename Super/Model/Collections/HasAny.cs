using Super.Model.Specifications;
using System.Collections;
using System.Collections.Generic;

namespace Super.Model.Collections
{
	public sealed class HasAny : DelegatedSpecification<ICollection>
	{
		public static HasAny Default { get; } = new HasAny();

		HasAny() : base(x => x.Count > 0) {}
	}

	public sealed class HasAny<T> : DelegatedSpecification<ICollection<T>>
	{
		public static HasAny<T> Default { get; } = new HasAny<T>();

		HasAny() : base(x => x.Count > 0) {}
	}
}
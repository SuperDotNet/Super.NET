using System.Collections.Generic;
using Super.Model.Specifications;

namespace Super.Model.Collections {
	public sealed class HasNone<T> : InverseSpecification<ICollection<T>>
	{
		public static HasNone<T> Default { get; } = new HasNone<T>();

		HasNone() : base(HasAny<T>.Default) {}
	}
}
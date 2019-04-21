using System.Collections.Generic;
using Super.Model.Selection.Conditions;

namespace Super.Model.Collections {
	public sealed class HasNone<T> : InverseCondition<ICollection<T>>
	{
		public static HasNone<T> Default { get; } = new HasNone<T>();

		HasNone() : base(HasAny<T>.Default) {}
	}
}
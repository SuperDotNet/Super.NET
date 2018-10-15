using Super.Model.Specifications;
using Super.Reflection.Types;

namespace Super.Runtime.Objects
{
	public sealed class CanCast<TFrom, TTo> : AllSpecification<TFrom>
	{
		public static CanCast<TFrom, TTo> Default { get; } = new CanCast<TFrom, TTo>();

		CanCast() : base(Start.Assigned<TFrom>(),
		                 Start.Metadata<TFrom>().Out(IsAssignableFrom<TTo>.Default)) {}
	}
}
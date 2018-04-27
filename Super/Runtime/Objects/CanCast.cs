using Super.Model.Specifications;
using Super.Reflection.Types;

namespace Super.Runtime.Objects
{
	public sealed class CanCast<TFrom, TTo> : AllSpecification<TFrom>
	{
		public static CanCast<TFrom, TTo> Default { get; } = new CanCast<TFrom, TTo>();

		CanCast() : base(IsAssigned<TFrom>.Default,
		                 In<TFrom>.Type()
		                          .Metadata()
		                          .Out(IsAssignableFrom<TTo>.Default)) {}
	}
}
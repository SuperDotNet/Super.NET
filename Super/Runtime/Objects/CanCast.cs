using Super.Model.Extents;
using Super.Model.Specifications;
using Super.Reflection;
using Super.Reflection.Types;

namespace Super.Runtime.Objects
{
	public sealed class CanCast<TFrom, TTo> : AllSpecification<TFrom>
	{
		public static CanCast<TFrom, TTo> Default { get; } = new CanCast<TFrom, TTo>();

		CanCast() : base(IsAssigned<TFrom>.Default,
		                 IsAssignableFrom<TTo>.Default
		                                      .In()
		                                      .Metadata()
		                                      .Type(I<TFrom>.Default)
		                                      .Return()) {}
	}
}
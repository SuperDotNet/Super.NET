using Super.Model.Specifications;
using Super.Reflection.Types;

namespace Super.Runtime.Objects
{
	public sealed class CanCast<TFrom, TTo> : DecoratedSpecification<TFrom>
	{
		public static CanCast<TFrom, TTo> Default { get; } = new CanCast<TFrom, TTo>();

		CanCast()
			: base(IsAssigned<TFrom>.Default
			                        .And(IsAssignableFrom<TTo>.Default
			                                                  .Select(TypeMetadataSelector.Default)
			                                                  .Select(InstanceTypeSelector<TFrom>.Default))) {}
	}
}
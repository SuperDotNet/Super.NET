using Super.ExtensionMethods;
using Super.Model.Specifications;
using Super.Reflection;

namespace Super.Runtime
{
	public sealed class CanCast<TFrom, TTo> : DecoratedSpecification<TFrom>
	{
		public static CanCast<TFrom, TTo> Default { get; } = new CanCast<TFrom, TTo>();

		CanCast()
			: base(IsAssigned<TFrom>.Default
			                        .And(IsAssignableFrom<TTo>.Default
			                                                  .Select(TypeMetadataCoercer.Default)
			                                                  .Select(InstanceTypeCoercer<TFrom>.Default))) {}
	}
}
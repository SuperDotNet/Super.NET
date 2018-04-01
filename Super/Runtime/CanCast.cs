using Super.ExtensionMethods;
using Super.Model.Specifications;
using Super.Reflection;

namespace Super.Runtime
{
	public sealed class CanCast<TFrom, TTo> : DecoratedSpecification<TFrom>
	{
		public static CanCast<TFrom, TTo> Default { get; } = new CanCast<TFrom, TTo>();

		CanCast()
			: base(AssignedSpecification<TFrom>
			       .Default.And(IsAssignableSpecification<TTo>
			                    .Default.Adapt(TypeMetadataCoercer.Default.In(InstanceTypeCoercer<TFrom>.Default)))) {}
	}
}
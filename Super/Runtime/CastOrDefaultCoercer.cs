using Super.ExtensionMethods;
using Super.Model.Sources;
using Super.Reflection;

namespace Super.Runtime
{
	sealed class CastOrDefaultCoercer<TFrom, TTo> : Conditional<TFrom, TTo>
	{
		public static CastOrDefaultCoercer<TFrom, TTo> Default { get; } = new CastOrDefaultCoercer<TFrom, TTo>();

		CastOrDefaultCoercer() : base(IsAssignableSpecification<TTo>.Default.Adapt()
		                                                            .In(TypeMetadataCoercer.Default)
		                                                            .In(InstanceTypeCoercer<TFrom>.Default)
		                                                            .ToSpecification(),
		                              CastCoercer<TFrom, TTo>.Default) {}
	}
}
using Super.ExtensionMethods;
using Super.Model.Collections;
using Super.Model.Specifications;
using Super.Reflection;
using System.Reflection;

namespace Super.Runtime.Activation
{
	sealed class HasSingleParameterConstructor<T> : DecoratedSpecification<ConstructorInfo>
	{
		public static HasSingleParameterConstructor<T> Default { get; } = new HasSingleParameterConstructor<T>();

		HasSingleParameterConstructor()
			: base(IsAssignableFrom<T>.Default
			                          .Select(TypeMetadataCoercer.Default.Assigned()) // TODO: Fix assigned.
			                          .Select(FirstOrDefaultCoercer<ParameterInfo>.Default
			                                                                      .Out(ParameterType.Default.Assigned()))
			                          .And(RemainingParametersAreOptional.Default)
			                          .Select(Parameters.Default)) {}
	}
}
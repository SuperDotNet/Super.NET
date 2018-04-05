using Super.ExtensionMethods;
using Super.Model.Collections;
using Super.Model.Specifications;
using Super.Reflection;
using System.Reflection;

namespace Super.Runtime.Activation
{
	sealed class HasSingleParameterConstructor<T> : DelegatedSpecification<ConstructorInfo>
	{
		public static HasSingleParameterConstructor<T> Default { get; } = new HasSingleParameterConstructor<T>();

		HasSingleParameterConstructor()
			: base(IsAssignableSpecification<T>.Default
			                                   .Adapt()
			                                   .In(TypeMetadataCoercer.Default)
			                                   .Assigned(FirstOrDefaultCoercer<ParameterInfo>.Default
			                                                                                 .Assigned(ParameterType.Default))
			                                   .ToSpecification()
			                                   .And(RemainingParametersAreOptional.Default)
			                                   .Adapt()
			                                   .In(Parameters.Default)
			                                   .Get) {}
	}
}
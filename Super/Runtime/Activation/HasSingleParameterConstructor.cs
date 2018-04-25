using Super.Model.Collections;
using Super.Model.Extents;
using Super.Model.Specifications;
using Super.Reflection.Members;
using Super.Reflection.Types;
using System.Reflection;

namespace Super.Runtime.Activation
{
	sealed class HasSingleParameterConstructor<T> : DelegatedSpecification<ConstructorInfo>
	{
		public static HasSingleParameterConstructor<T> Default { get; } = new HasSingleParameterConstructor<T>();

		HasSingleParameterConstructor()
			: base(Parameters.Default
			                 .Out(FirstOrDefaultSelector<ParameterInfo>.Default
			                                                           .Out(ParameterType.Default
			                                                                             .Out(x => x.Metadata())
			                                                                             .Assigned())
			                                                           .Out()
			                                                           .Out(IsAssignableFrom<T>.Default.IsSatisfiedBy)
			                                                           .And(RemainingParametersAreOptional.Default.In())
			                                                           .Get())
			                 .Get) {}
	}
}
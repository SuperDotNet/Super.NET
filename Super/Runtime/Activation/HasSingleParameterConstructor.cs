using Super.Model.Selection;
using Super.Model.Specifications;
using Super.Reflection.Members;
using Super.Reflection.Types;
using System.Collections.Generic;
using System.Reflection;

namespace Super.Runtime.Activation
{
	sealed class HasSingleParameterConstructor<T> : DecoratedSpecification<ConstructorInfo>
	{
		public static HasSingleParameterConstructor<T> Default { get; } = new HasSingleParameterConstructor<T>();

		HasSingleParameterConstructor() : this(Parameters.Default) {}

		public HasSingleParameterConstructor(ISelect<ConstructorInfo, IEnumerable<ParameterInfo>> parameters)
			: base(parameters.Access()
			                 .FirstAssigned()
			                 .Out(ParameterType.Default
			                                   .Select(TypeMetadata.Default)
			                                   .Select(IsAssignableFrom<T>.Default)
			                                   .Assigned())
			                 .And(parameters.Out(RemainingParametersAreOptional.Default))) {}
	}
}
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

		public HasSingleParameterConstructor(ISelect<ConstructorInfo, ICollection<ParameterInfo>> parameters)
			: base(parameters.FirstAssigned()
			                 .Out(ParameterType.Default
			                                   .Metadata()
			                                   .Out(IsAssignableFrom<T>.Default)
			                                   .Assigned())
			                 .And(parameters.Out(RemainingParametersAreOptional.Default))
			                 .Exit()) {}
	}
}
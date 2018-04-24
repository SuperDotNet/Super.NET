﻿using Super.Model.Collections;
using Super.Model.Extents;
using Super.Model.Specifications;
using Super.Reflection.Members;
using Super.Reflection.Types;
using System.Reflection;

namespace Super.Runtime.Activation
{
	sealed class HasSingleParameterConstructor<T> : DecoratedSpecification<ConstructorInfo>
	{
		public static HasSingleParameterConstructor<T> Default { get; } = new HasSingleParameterConstructor<T>();

		HasSingleParameterConstructor()
			: base(IsAssignableFrom<T>.Default
			                          .In(FirstOrDefaultSelector<ParameterInfo>
			                                  .Default.Out(ParameterType.Default
			                                                            .Out(TypeMetadataSelector.Default)
			                                                            .Assigned()))
			                          .And(RemainingParametersAreOptional.Default)
			                          .In(Parameters.Default)) {}
	}
}
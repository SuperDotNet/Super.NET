using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Super.ExtensionMethods;
using Super.Model.Specifications;

namespace Super.Reflection
{
	sealed class RemainingParametersAreOptional : ISpecification<IEnumerable<ParameterInfo>>
	{
		public static RemainingParametersAreOptional Default { get; } =
			new RemainingParametersAreOptional();

		RemainingParametersAreOptional() {}

		public bool IsSatisfiedBy(IEnumerable<ParameterInfo> parameter)
			=> parameter.Skip(1).All(x => x.IsOptional || x.Has<ParamArrayAttribute>());
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Super.Model.Specifications;

namespace Super.Reflection.Members
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
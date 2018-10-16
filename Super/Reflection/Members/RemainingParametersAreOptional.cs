using Super.Model.Sequences;
using Super.Model.Specifications;
using System;
using System.Linq;
using System.Reflection;

namespace Super.Reflection.Members
{
	sealed class RemainingParametersAreOptional : ISpecification<Array<ParameterInfo>>
	{
		public static RemainingParametersAreOptional Default { get; } =
			new RemainingParametersAreOptional();

		RemainingParametersAreOptional() {}

		public bool IsSatisfiedBy(Array<ParameterInfo> parameter)
			=> parameter.Reference().Skip(1).All(x => x.IsOptional || x.Has<ParamArrayAttribute>());
	}
}
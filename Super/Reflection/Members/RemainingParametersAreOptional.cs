using Super.Model.Sequences;
using System;
using System.Linq;
using System.Reflection;
using Super.Model.Selection.Conditions;

namespace Super.Reflection.Members
{
	sealed class RemainingParametersAreOptional : ICondition<Array<ParameterInfo>>
	{
		public static RemainingParametersAreOptional Default { get; } =
			new RemainingParametersAreOptional();

		RemainingParametersAreOptional() {}

		public bool Get(Array<ParameterInfo> parameter)
			=> parameter.Open().Skip(1).All(x => x.IsOptional || x.Has<ParamArrayAttribute>());
	}
}
using Super.Compose;
using Super.Model.Selection.Conditions;
using System;
using System.Reflection;

namespace Super.Reflection.Members
{
	sealed class ConstructorCondition : DelegatedCondition<ConstructorInfo>
	{
		public static ConstructorCondition Default { get; } = new ConstructorCondition();

		ConstructorCondition()
			: base(Start.An.Instance(Parameters.Default)
			            .Open()
			            .Then()
			            .To(x => x.HasNone()
			                      .Or(x.AllAre(y => y.IsOptional || y.Has<ParamArrayAttribute>())))) {}
	}
}
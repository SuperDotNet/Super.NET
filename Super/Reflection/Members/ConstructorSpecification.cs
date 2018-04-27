using Super.Model.Specifications;
using System;
using System.Reflection;

namespace Super.Reflection.Members
{
	sealed class ConstructorSpecification : DecoratedSpecification<ConstructorInfo>
	{
		public static ConstructorSpecification Default { get; } = new ConstructorSpecification();

		ConstructorSpecification() : base(Parameters.Default
		                                            .To(x => x.HasNone()
		                                                      .Or(x.AllAre(y => y.IsOptional ||
		                                                                        y.Has<ParamArrayAttribute>())))) {}
	}
}
using Super.ExtensionMethods;
using Super.Model.Collections;
using Super.Model.Specifications;
using System;
using System.Reflection;

namespace Super.Reflection
{
	sealed class ConstructorSpecification : DecoratedSpecification<ConstructorInfo>
	{
		public static ConstructorSpecification Default { get; } = new ConstructorSpecification();

		ConstructorSpecification()
			: base(HasNone<ParameterInfo>.Default
			                             .Or(new AllItemsAre<ParameterInfo>(x => x.IsOptional ||
			                                                                     x.Has<ParamArrayAttribute>()))
			                             .Select(Parameters.Default)) {}
	}
}
using System;
using System.Reflection;
using Super.ExtensionMethods;
using Super.Model.Collections;
using Super.Model.Specifications;

namespace Super.Reflection.Members
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
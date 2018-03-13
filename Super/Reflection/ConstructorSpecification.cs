using System;
using System.Linq;
using System.Reflection;
using Super.ExtensionMethods;
using Super.Model.Specifications;

namespace Super.Reflection
{
	sealed class ConstructorSpecification : ISpecification<ConstructorInfo>
	{
		public static ConstructorSpecification Default { get; } = new ConstructorSpecification();

		ConstructorSpecification() {}

		public bool IsSatisfiedBy(ConstructorInfo parameter)
		{
			var parameters = parameter.GetParameters();
			var result = parameters.Length == 0 ||
			             parameters.All(x => x.IsOptional || x.Has<ParamArrayAttribute>());
			return result;
		}
	}
}
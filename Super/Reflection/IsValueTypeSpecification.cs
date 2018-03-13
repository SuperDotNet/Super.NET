using System.Reflection;
using Super.Model.Specifications;

namespace Super.Reflection
{
	sealed class IsValueTypeSpecification : ISpecification<TypeInfo>
	{
		public static IsValueTypeSpecification Default { get; } = new IsValueTypeSpecification();

		IsValueTypeSpecification() {}

		public bool IsSatisfiedBy(TypeInfo parameter) => parameter.IsValueType;
	}
}
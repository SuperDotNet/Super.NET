using System.Reflection;
using Super.Model.Specifications;

namespace Super.Reflection
{
	sealed class IsGenericType : ISpecification<TypeInfo>
	{
		public static IsGenericType Default { get; } = new IsGenericType();

		IsGenericType() {}

		public bool IsSatisfiedBy(TypeInfo parameter) => parameter.IsGenericType;
	}
}
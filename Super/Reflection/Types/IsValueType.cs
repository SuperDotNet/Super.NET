using Super.Model.Specifications;
using System.Reflection;

namespace Super.Reflection.Types
{
	sealed class IsValueType : ISpecification<TypeInfo>
	{
		public static IsValueType Default { get; } = new IsValueType();

		IsValueType() {}

		public bool IsSatisfiedBy(TypeInfo parameter) => parameter.IsValueType;
	}
}
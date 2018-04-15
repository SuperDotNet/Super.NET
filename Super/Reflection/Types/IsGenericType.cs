using System.Reflection;
using Super.Model.Specifications;

namespace Super.Reflection.Types
{
	sealed class IsGenericType : DelegatedSpecification<TypeInfo>
	{
		public static IsGenericType Default { get; } = new IsGenericType();

		IsGenericType() : base(x => x.IsGenericType) {}
	}
}
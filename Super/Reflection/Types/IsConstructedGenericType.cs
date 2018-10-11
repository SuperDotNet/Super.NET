using Super.Model.Specifications;
using System;

namespace Super.Reflection.Types
{
	sealed class IsConstructedGenericType : DelegatedSpecification<Type>
	{
		public static IsConstructedGenericType Default { get; } = new IsConstructedGenericType();

		IsConstructedGenericType() : base(x => x.IsConstructedGenericType) {}
	}

	sealed class IsDefinedGenericType : AllSpecification<Type>
	{
		public static IsDefinedGenericType Default { get; } = new IsDefinedGenericType();

		IsDefinedGenericType() : base(IsConstructedGenericType.Default, IsGenericTypeDefinition.Default.Inverse()) {}
	}
}
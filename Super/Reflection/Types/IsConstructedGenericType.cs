using System;
using Super.Model.Specifications;

namespace Super.Reflection.Types
{
	sealed class IsConstructedGenericType : DelegatedSpecification<Type>
	{
		public static IsConstructedGenericType Default { get; } = new IsConstructedGenericType();

		IsConstructedGenericType() : base(x => x.IsConstructedGenericType) {}
	}
}
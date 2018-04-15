using System;
using Super.Model.Specifications;

namespace Super.Reflection.Types
{
	sealed class IsGenericTypeDefinition : DelegatedSpecification<Type>
	{
		public static IsGenericTypeDefinition Default { get; } = new IsGenericTypeDefinition();

		IsGenericTypeDefinition() : base(x => x.IsGenericTypeDefinition) {}
	}
}
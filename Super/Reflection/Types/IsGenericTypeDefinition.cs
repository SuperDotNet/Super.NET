using Super.Model.Specifications;
using System;

namespace Super.Reflection.Types
{
	sealed class IsGenericTypeDefinition : DelegatedSpecification<Type>
	{
		public static IsGenericTypeDefinition Default { get; } = new IsGenericTypeDefinition();

		IsGenericTypeDefinition() : base(x => x.IsGenericTypeDefinition) {}
	}
}
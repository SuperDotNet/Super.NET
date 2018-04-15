using System;
using Super.Model.Specifications;

namespace Super.Reflection.Types
{
	sealed class IsClass : DelegatedSpecification<Type>
	{
		public static IsClass Default { get; } = new IsClass();

		IsClass() : base(x => x.IsClass) {}
	}
}
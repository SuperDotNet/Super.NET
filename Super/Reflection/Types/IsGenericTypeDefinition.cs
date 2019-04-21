using System;
using Super.Model.Selection.Conditions;

namespace Super.Reflection.Types
{
	sealed class IsGenericTypeDefinition : DelegatedCondition<Type>
	{
		public static IsGenericTypeDefinition Default { get; } = new IsGenericTypeDefinition();

		IsGenericTypeDefinition() : base(x => x.IsGenericTypeDefinition) {}
	}
}
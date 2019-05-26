using System;
using Super.Model.Selection.Conditions;

namespace Super.Reflection.Types
{
	sealed class IsDefinedGenericType : AllCondition<Type>
	{
		public static IsDefinedGenericType Default { get; } = new IsDefinedGenericType();

		IsDefinedGenericType() : base(IsConstructedGenericType.Default,
		                              IsGenericTypeDefinition.Default.Then().Inverse().Get()) {}
	}
}
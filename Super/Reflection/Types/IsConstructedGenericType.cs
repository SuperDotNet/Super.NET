using Super.Model.Selection.Conditions;
using System;

namespace Super.Reflection.Types
{
	sealed class IsConstructedGenericType : Condition<Type>
	{
		public static IsConstructedGenericType Default { get; } = new IsConstructedGenericType();

		IsConstructedGenericType() : base(x => x.IsConstructedGenericType) {}
	}

	sealed class IsDefinedGenericType : AllCondition<Type>
	{
		public static IsDefinedGenericType Default { get; } = new IsDefinedGenericType();

		IsDefinedGenericType() : base(IsConstructedGenericType.Default,
		                              IsGenericTypeDefinition.Default.Then().Inverse().Get()) {}
	}
}
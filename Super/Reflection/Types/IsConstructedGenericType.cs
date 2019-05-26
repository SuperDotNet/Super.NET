using System;
using Super.Model.Selection.Conditions;

namespace Super.Reflection.Types
{
	sealed class IsConstructedGenericType : Condition<Type>
	{
		public static IsConstructedGenericType Default { get; } = new IsConstructedGenericType();

		IsConstructedGenericType() : base(x => x.IsConstructedGenericType) {}
	}
}
using Super.Reflection.Types;
using Super.Runtime;
using System;
using System.Reflection;
using Super.Model.Selection.Conditions;

namespace Super.Reflection
{
	sealed class IsAssignableStructure : IsAssigned<Type, Type>
	{
		public static IsAssignableStructure Default { get; } = new IsAssignableStructure();

		IsAssignableStructure() : base(Nullable.GetUnderlyingType) {}
	}

	sealed class IsReference : InverseCondition<TypeInfo>
	{
		public static IsReference Default { get; } = new IsReference();
		IsReference() : base(IsValueType.Default) {}
	}

	/*sealed class CanBeAssigned : AnySpecification<TypeInfo>
	{
		public static CanBeAssigned Default { get; } = new CanBeAssigned();

		CanBeAssigned() : base(IsAssignableStructure.Default, IsReference.Default) {}
	}*/

}
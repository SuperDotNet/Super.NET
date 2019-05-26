using System;
using Super.Runtime;

namespace Super.Reflection
{
	sealed class IsAssignableStructure : IsAssigned<Type, Type>
	{
		public static IsAssignableStructure Default { get; } = new IsAssignableStructure();

		IsAssignableStructure() : base(Nullable.GetUnderlyingType) {}
	}

	/*sealed class CanBeAssigned : AnySpecification<TypeInfo>
	{
		public static CanBeAssigned Default { get; } = new CanBeAssigned();

		CanBeAssigned() : base(IsAssignableStructure.Default, IsReference.Default) {}
	}*/
}
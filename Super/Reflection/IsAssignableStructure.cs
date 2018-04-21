using Super.Model.Specifications;
using Super.Reflection.Types;
using Super.Runtime;
using System;
using System.Reflection;

namespace Super.Reflection
{
	sealed class IsAssignableStructure : IsModified<Type, Type>
	{
		public static IsAssignableStructure Default { get; } = new IsAssignableStructure();

		IsAssignableStructure() : base(Nullable.GetUnderlyingType) {}
	}

	sealed class IsReference : InverseSpecification<TypeInfo>
	{
		public static IsReference Default { get; } = new IsReference();
		IsReference() : base(IsValueType.Default) {}
	}

	sealed class CanBeAssigned : AnySpecification<TypeInfo>
	{
		public static CanBeAssigned Default { get; } = new CanBeAssigned();

		CanBeAssigned() : base(IsAssignableStructure.Default, IsReference.Default) {}
	}

}
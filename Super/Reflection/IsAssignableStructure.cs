using Super.Runtime;
using System;

namespace Super.Reflection
{
	sealed class IsAssignableStructure : IsAssigned<Type, Type>
	{
		public static IsAssignableStructure Default { get; } = new IsAssignableStructure();

		IsAssignableStructure() : base(Nullable.GetUnderlyingType) {}
	}
}
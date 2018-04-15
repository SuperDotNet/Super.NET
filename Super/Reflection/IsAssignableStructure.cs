using Super.Model.Specifications;
using System;

namespace Super.Reflection
{
	sealed class IsAssignableStructure : HasResult<Type, Type>
	{
		public static IsAssignableStructure Default { get; } = new IsAssignableStructure();

		IsAssignableStructure() : base(Nullable.GetUnderlyingType) {}
	}
}
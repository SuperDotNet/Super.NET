using System;
using Super.Model.Specifications;

namespace Super.Reflection
{
	sealed class AssignableStructureSpecification : HasResult<Type, Type>
	{
		public static AssignableStructureSpecification Default { get; } = new AssignableStructureSpecification();

		AssignableStructureSpecification() : base(Nullable.GetUnderlyingType) {}
	}
}
using System.Collections.Generic;

namespace Super.Reflection
{
	public sealed class ImplementsGenericEnumerable : ImplementsGenericType
	{
		public static ImplementsGenericEnumerable Default { get; } = new ImplementsGenericEnumerable();

		ImplementsGenericEnumerable() : base(typeof(IEnumerable<>)) {}
	}
}
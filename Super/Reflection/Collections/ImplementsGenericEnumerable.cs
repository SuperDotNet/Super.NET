using System.Collections.Generic;
using System.Reflection;
using Super.Reflection.Types;

namespace Super.Reflection.Collections
{
	public sealed class ImplementsGenericEnumerable : ImplementsGenericType
	{
		public static ImplementsGenericEnumerable Default { get; } = new ImplementsGenericEnumerable();

		ImplementsGenericEnumerable() : base(typeof(IEnumerable<>).GetTypeInfo()) {}
	}
}
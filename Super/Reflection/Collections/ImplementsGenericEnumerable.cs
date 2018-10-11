using Super.Reflection.Types;
using System.Collections.Generic;
using System.Reflection;

namespace Super.Reflection.Collections
{
	public sealed class ImplementsGenericEnumerable : ImplementsGenericType
	{
		public static ImplementsGenericEnumerable Default { get; } = new ImplementsGenericEnumerable();

		ImplementsGenericEnumerable() : base(typeof(IEnumerable<>).GetTypeInfo()) {}
	}
}
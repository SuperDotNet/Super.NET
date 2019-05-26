using System;
using System.Collections.Generic;
using Super.Model.Sequences;

namespace Super.Reflection.Selection
{
	public sealed class TypesInSameNamespace : ArrayInstance<Type>
	{
		public TypesInSameNamespace(Type referenceType, IEnumerable<Type> candidates)
			: base(candidates.Introduce(referenceType.Namespace, x => x.Item1.Namespace == x.Item2)) {}
	}
}
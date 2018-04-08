using System;
using System.Collections.Generic;
using System.Linq;
using Super.Model.Collections;

namespace Super.Reflection.Selection
{
	public sealed class TypesInSameNamespace : Items<Type>
	{
		public TypesInSameNamespace(Type referenceType, IEnumerable<Type> candidates) :
			base(candidates.Where(x => x.Namespace == referenceType.Namespace)) {}
	}
}
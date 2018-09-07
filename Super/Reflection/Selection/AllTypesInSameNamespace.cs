using Super.Model.Collections;
using System;
using System.Collections.Generic;

namespace Super.Reflection.Selection
{
	public sealed class AllTypesInSameNamespace<T> : Array<Type>
	{
		public AllTypesInSameNamespace() : this(new AllTypesInSameNamespace(typeof(T)).AsEnumerable()) {}

		public AllTypesInSameNamespace(IEnumerable<Type> items) : base(items) {}
	}

	public sealed class AllTypesInSameNamespace : Array<Type>
	{
		public AllTypesInSameNamespace(Type referenceType)
			: this(new TypesInSameNamespace(referenceType, new AllAssemblyTypes(referenceType).AsEnumerable()).AsEnumerable()) {}

		public AllTypesInSameNamespace(IEnumerable<Type> items) : base(items) {}
	}
}
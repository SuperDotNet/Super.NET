using Super.Model.Collections;
using System;
using System.Collections.Generic;

namespace Super.Reflection.Selection
{
	public sealed class AllTypesInSameNamespace<T> : Arrays<Type>
	{
		public AllTypesInSameNamespace() : this(new AllTypesInSameNamespace(typeof(T)).AsEnumerable()) {}

		public AllTypesInSameNamespace(IEnumerable<Type> items) : base(items) {}
	}

	public sealed class AllTypesInSameNamespace : Arrays<Type>
	{
		public AllTypesInSameNamespace(Type referenceType)
			: this(new TypesInSameNamespace(referenceType, new AllAssemblyTypes(referenceType).AsEnumerable()).AsEnumerable()) {}

		public AllTypesInSameNamespace(IEnumerable<Type> items) : base(items) {}
	}
}
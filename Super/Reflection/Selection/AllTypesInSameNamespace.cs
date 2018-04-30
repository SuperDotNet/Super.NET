using Super.Model.Collections;
using System;
using System.Collections.Generic;

namespace Super.Reflection.Selection
{
	public sealed class AllTypesInSameNamespace<T> : Items<Type>
	{
		public AllTypesInSameNamespace() : this(new AllTypesInSameNamespace(typeof(T))) {}

		public AllTypesInSameNamespace(IEnumerable<Type> items) : base(items) {}
	}

	public sealed class AllTypesInSameNamespace : Items<Type>
	{
		public AllTypesInSameNamespace(Type referenceType)
			: this(new TypesInSameNamespace(referenceType, new AllAssemblyTypes(referenceType))) {}

		public AllTypesInSameNamespace(IEnumerable<Type> items) : base(items) {}
	}
}
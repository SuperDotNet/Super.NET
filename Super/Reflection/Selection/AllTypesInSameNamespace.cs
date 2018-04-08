using System;
using Super.Model.Collections;

namespace Super.Reflection.Selection
{
	public sealed class AllTypesInSameNamespace<T> : Items<Type>
	{
		public AllTypesInSameNamespace() : base(new AllTypesInSameNamespace(typeof(T))) {}
	}

	public sealed class AllTypesInSameNamespace : Items<Type>
	{
		public AllTypesInSameNamespace(Type referenceType)
			: base(new TypesInSameNamespace(referenceType, new AllAssemblyTypes(referenceType))) {}
	}
}
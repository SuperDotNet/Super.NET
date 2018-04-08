using System;
using Super.Model.Collections;

namespace Super.Reflection.Selection
{
	public sealed class PublicTypesInSameNamespace<T> : Items<Type>
	{
		public PublicTypesInSameNamespace() : base(new PublicTypesInSameNamespace(typeof(T))) {}
	}

	public sealed class PublicTypesInSameNamespace : Items<Type>
	{
		public PublicTypesInSameNamespace(Type referenceType)
			: base(new TypesInSameNamespace(referenceType, new PublicAssemblyTypes(referenceType))) {}
	}
}
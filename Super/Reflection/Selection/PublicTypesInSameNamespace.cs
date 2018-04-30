using Super.Model.Collections;
using System;
using System.Collections.Generic;

namespace Super.Reflection.Selection
{
	public sealed class PublicTypesInSameNamespace<T> : Items<Type>
	{
		public PublicTypesInSameNamespace() : this(new PublicTypesInSameNamespace(typeof(T))) {}

		public PublicTypesInSameNamespace(IEnumerable<Type> items) : base(items) {}
	}

	public sealed class PublicTypesInSameNamespace : Items<Type>
	{
		public PublicTypesInSameNamespace(Type referenceType)
			: this(new TypesInSameNamespace(referenceType, new PublicAssemblyTypes(referenceType))) {}

		public PublicTypesInSameNamespace(IEnumerable<Type> items) : base(items) {}
	}
}
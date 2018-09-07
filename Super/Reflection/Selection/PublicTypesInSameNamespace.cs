using Super.Model.Collections;
using System;
using System.Collections.Generic;

namespace Super.Reflection.Selection
{
	public sealed class PublicTypesInSameNamespace<T> : Array<Type>
	{
		public PublicTypesInSameNamespace() : this(new PublicTypesInSameNamespace(typeof(T)).Get().AsEnumerable()) {}

		public PublicTypesInSameNamespace(IEnumerable<Type> items) : base(items) {}
	}

	public sealed class PublicTypesInSameNamespace : Array<Type>
	{
		public PublicTypesInSameNamespace(Type referenceType)
			: this(new TypesInSameNamespace(referenceType, new PublicAssemblyTypes(referenceType).Get().AsEnumerable()).Get().AsEnumerable()) {}

		public PublicTypesInSameNamespace(IEnumerable<Type> items) : base(items) {}
	}
}
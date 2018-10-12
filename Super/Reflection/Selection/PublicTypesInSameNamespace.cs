using Super.Model.Collections;
using System;

namespace Super.Reflection.Selection
{
	public sealed class PublicTypesInSameNamespace<T> : DecoratedArray<Type>
	{
		public static PublicTypesInSameNamespace<T> Default { get; } = new PublicTypesInSameNamespace<T>();

		PublicTypesInSameNamespace() : base(new PublicTypesInSameNamespace(typeof(T))) {}
	}

	public sealed class PublicTypesInSameNamespace : DecoratedArray<Type>
	{
		public PublicTypesInSameNamespace(Type referenceType)
			: base(new TypesInSameNamespace(referenceType, new PublicAssemblyTypes(referenceType).Get().Reference())) {}
	}
}
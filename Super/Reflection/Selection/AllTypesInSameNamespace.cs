using Super.Model.Sequences;
using System;

namespace Super.Reflection.Selection
{
	public sealed class AllTypesInSameNamespace<T> : DecoratedArray<Type>
	{
		public static AllTypesInSameNamespace<T> Default { get; } = new AllTypesInSameNamespace<T>();

		AllTypesInSameNamespace() : base(new AllTypesInSameNamespace(typeof(T))) {}
	}

	public sealed class AllTypesInSameNamespace : DecoratedArray<Type>
	{
		public AllTypesInSameNamespace(Type referenceType)
			: base(new TypesInSameNamespace(referenceType, new AllAssemblyTypes(referenceType).Get().Open())) {}

	}
}
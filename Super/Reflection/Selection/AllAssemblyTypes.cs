using Super.Model.Sequences;
using System;

namespace Super.Reflection.Selection
{
	public sealed class AllAssemblyTypes : ArrayInstance<Type>
	{
		public AllAssemblyTypes(Type referenceType) : base(referenceType.Assembly.DefinedTypes) {}
	}

	public sealed class AllAssemblyTypes<T> : DecoratedArray<Type>
	{
		public static AllAssemblyTypes<T> Default { get; } = new AllAssemblyTypes<T>();

		AllAssemblyTypes() : base(new AllAssemblyTypes(typeof(T))) {}
	}
}
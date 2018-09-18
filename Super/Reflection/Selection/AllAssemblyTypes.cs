using Super.Model.Collections;
using System;

namespace Super.Reflection.Selection
{
	public sealed class AllAssemblyTypes : Arrays<Type>
	{
		public AllAssemblyTypes(Type referenceType) : base(referenceType.Assembly.DefinedTypes) {}
	}

	public sealed class AllAssemblyTypes<T> : Arrays<Type>
	{
		public static AllAssemblyTypes<T> Default { get; } = new AllAssemblyTypes<T>();

		AllAssemblyTypes() : base(new AllAssemblyTypes(typeof(T)).Get().AsEnumerable()) {}
	}
}
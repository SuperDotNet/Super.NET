using Super.Model.Collections;
using System;

namespace Super.Reflection.Selection
{
	public sealed class AllAssemblyTypes : Array<Type>
	{
		public AllAssemblyTypes(Type referenceType) : base(referenceType.Assembly.DefinedTypes) {}
	}

	public sealed class AllAssemblyTypes<T> : Array<Type>
	{
		public static AllAssemblyTypes<T> Default { get; } = new AllAssemblyTypes<T>();

		AllAssemblyTypes() : base(new AllAssemblyTypes(typeof(T)).Get().AsEnumerable()) {}
	}
}
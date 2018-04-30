using Super.Model.Collections;
using System;

namespace Super.Reflection.Selection
{
	public sealed class AllAssemblyTypes : Items<Type>
	{
		public AllAssemblyTypes(Type referenceType) : base(referenceType.Assembly.DefinedTypes.ToTypes()) {}
	}

	public sealed class AllAssemblyTypes<T> : Items<Type>
	{
		public static AllAssemblyTypes<T> Default { get; } = new AllAssemblyTypes<T>();

		AllAssemblyTypes() : base(new AllAssemblyTypes(typeof(T)).Get()) {}
	}
}
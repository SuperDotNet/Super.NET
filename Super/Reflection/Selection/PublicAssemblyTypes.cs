using System;
using System.Reflection;
using Super.Model.Sequences;
using Super.Runtime.Activation;

namespace Super.Reflection.Selection
{
	public sealed class PublicAssemblyTypes<T> : ArrayResult<Type>
	{
		public static PublicAssemblyTypes<T> Default { get; } = new PublicAssemblyTypes<T>();

		PublicAssemblyTypes() : base(new PublicAssemblyTypes(typeof(T))) {}
	}

	public sealed class PublicAssemblyTypes : ArrayInstance<Type>, IActivateUsing<Assembly>, IActivateUsing<Type>
	{
		public PublicAssemblyTypes(Type referenceType) : this(referenceType.Assembly) {}

		public PublicAssemblyTypes(Assembly assembly) : base(assembly.ExportedTypes) {}
	}
}
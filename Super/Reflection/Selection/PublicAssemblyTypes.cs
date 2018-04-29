using Super.Model.Collections;
using Super.Runtime.Activation;
using System;
using System.Reflection;

namespace Super.Reflection.Selection
{
	public sealed class PublicAssemblyTypes<T> : Enumerable<Type>
	{
		public static PublicAssemblyTypes<T> Default { get; } = new PublicAssemblyTypes<T>();

		PublicAssemblyTypes() : base(new PublicAssemblyTypes(typeof(T))) {}
	}

	public sealed class PublicAssemblyTypes : Enumerable<Type>, IActivateMarker<Assembly>, IActivateMarker<Type>
	{
		public PublicAssemblyTypes(Type referenceType) : this(referenceType.Assembly) {}

		public PublicAssemblyTypes(Assembly assembly) : base(assembly.ExportedTypes) {}
	}
}
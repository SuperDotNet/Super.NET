using System;
using System.Reflection;
using Super.Model.Collections;

namespace Super.Reflection.Selection
{
	public sealed class NestedTypes<T> : Items<Type>
	{
		public NestedTypes() : base(new NestedTypes(typeof(T))) {}
	}

	public sealed class NestedTypes : Items<Type>
	{
		public NestedTypes(Type referenceType) : base(referenceType.GetTypeInfo()
		                                                           .DeclaredNestedTypes.ToTypes()) {}
	}
}
using Super.Model.Collections;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Super.Reflection.Selection
{
	public sealed class NestedTypes<T> : Arrays<Type>
	{
		public NestedTypes() : this(new NestedTypes(typeof(T)).Get()
		                                                      .AsEnumerable()) {}

		public NestedTypes(IEnumerable<Type> items) : base(items) {}
	}

	public sealed class NestedTypes : Arrays<Type>
	{
		public NestedTypes(Type referenceType) : base(referenceType.GetTypeInfo()
		                                                           .DeclaredNestedTypes) {}
	}
}
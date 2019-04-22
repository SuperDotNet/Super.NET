using Super.Model.Sequences;
using Super.Reflection.Types;
using System;
using System.Reflection;

namespace Super.Reflection.Selection
{
	public sealed class NestedTypes<T> : ArrayResult<Type>
	{
		public static NestedTypes<T> Default { get; } = new NestedTypes<T>();

		NestedTypes() : base(new NestedTypes(Type<T>.Metadata)) {}
	}

	public sealed class NestedTypes : ArrayInstance<Type>
	{
		public NestedTypes(TypeInfo referenceType) : base(referenceType.DeclaredNestedTypes) {}
	}
}
using Super.Model.Sequences;
using Super.Reflection.Types;
using System;

namespace Super.Reflection.Selection
{
	public sealed class PublicNestedTypes<T> : DecoratedArray<Type>
	{
		public static PublicNestedTypes<T> Default { get; } = new PublicNestedTypes<T>();

		PublicNestedTypes() : base(new PublicNestedTypes(Type<T>.Instance)) {}
	}

	public sealed class PublicNestedTypes : ArrayInstance<Type>
	{
		public PublicNestedTypes(Type referenceType) : base(referenceType.GetNestedTypes()) {}
	}
}
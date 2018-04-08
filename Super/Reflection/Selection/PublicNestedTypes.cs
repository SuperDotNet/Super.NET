using System;
using Super.Model.Collections;

namespace Super.Reflection.Selection
{
	public sealed class PublicNestedTypes<T> : Items<Type>
	{
		public PublicNestedTypes() : base(new PublicNestedTypes(typeof(T))) {}
	}

	public sealed class PublicNestedTypes : Items<Type>
	{
		public PublicNestedTypes(Type referenceType) : base(referenceType.GetNestedTypes()) {}
	}
}
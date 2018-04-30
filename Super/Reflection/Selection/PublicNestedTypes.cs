using Super.Model.Collections;
using System;
using System.Collections.Generic;

namespace Super.Reflection.Selection
{
	public sealed class PublicNestedTypes<T> : Items<Type>
	{
		public PublicNestedTypes() : this(new PublicNestedTypes(typeof(T))) {}

		public PublicNestedTypes(IEnumerable<Type> items) : base(items) {}
	}

	public sealed class PublicNestedTypes : Items<Type>
	{
		public PublicNestedTypes(Type referenceType) : base(referenceType.GetNestedTypes()) {}
	}
}
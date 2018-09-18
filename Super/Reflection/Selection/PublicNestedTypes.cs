﻿using Super.Model.Collections;
using System;
using System.Collections.Generic;

namespace Super.Reflection.Selection
{
	public sealed class PublicNestedTypes<T> : Arrays<Type>
	{
		public PublicNestedTypes() : this(new PublicNestedTypes(typeof(T)).Get().AsEnumerable()) {}

		public PublicNestedTypes(IEnumerable<Type> items) : base(items) {}
	}

	public sealed class PublicNestedTypes : Arrays<Type>
	{
		public PublicNestedTypes(Type referenceType) : base(referenceType.GetNestedTypes()) {}
	}
}
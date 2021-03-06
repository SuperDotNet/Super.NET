﻿using System;
using System.Reflection;
using Super.Reflection.Types;

namespace Super.Model.Selection.Adapters
{
	public class TypeSelector<T> : Selector<T, Type>
	{
		public TypeSelector(ISelect<T, Type> subject) : base(subject) {}

		public Selector<T, TypeInfo> Metadata() => Select(TypeMetadata.Default);
	}
}
﻿using Super.Model.Specifications;
using Super.Runtime.Objects;
using System;
using System.Reflection;

namespace Super.Reflection
{
	sealed class ContainedAttribute<TAttribute, T> : Attribute<TAttribute, T> where TAttribute : Attribute
	{
		readonly static Func<TAttribute, T> Select = ValueAwareCast<TAttribute, T>.Default.Get;

		public static ContainedAttribute<TAttribute, T> Default { get; } = new ContainedAttribute<TAttribute, T>();

		public static ContainedAttribute<TAttribute, T> Inherited { get; }
			= new ContainedAttribute<TAttribute, T>(Attribute<TAttribute>.Inherited);

		ContainedAttribute() : this(Attribute<TAttribute>.Default) {}

		public ContainedAttribute(IAttribute<TAttribute> attribute) : this(attribute, Select) {}

		public ContainedAttribute(IAttribute<TAttribute> attribute, Func<TAttribute, T> select)
			: this(IsContainedAttribute<TAttribute, T>.Default, attribute, select) {}

		public ContainedAttribute(ISpecification<ICustomAttributeProvider> specification,
		                          IAttribute<TAttribute> attribute, Func<TAttribute, T> select) : base(specification,
		                                                                                               attribute, select) {}
	}
}
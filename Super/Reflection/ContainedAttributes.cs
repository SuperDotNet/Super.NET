using Super.Model.Selection;
using Super.Model.Specifications;
using System;
using System.Reflection;

namespace Super.Reflection
{
	sealed class ContainedAttributes<TAttribute, T> : Attributes<TAttribute, T> where TAttribute : Attribute
	{
		readonly static Func<TAttribute, T> Select = In<TAttribute>.CastForValue<T>().Get;

		public static ContainedAttributes<TAttribute, T> Default { get; } = new ContainedAttributes<TAttribute, T>();

		public static ContainedAttributes<TAttribute, T> Inherited { get; }
			= new ContainedAttributes<TAttribute, T>(Declared<TAttribute>.Inherited);

		ContainedAttributes() : this(Declared<TAttribute>.Default) {}

		public ContainedAttributes(IDeclared<TAttribute> attribute) : this(attribute, Select) {}

		public ContainedAttributes(IDeclared<TAttribute> attribute, Func<TAttribute, T> select)
			: this(IsContainedAttribute<TAttribute>.Default, attribute, select) {}

		public ContainedAttributes(ISpecification<ICustomAttributeProvider> specification,
		                           IDeclared<TAttribute> attribute, Func<TAttribute, T> select)
			: base(specification, attribute, select) {}
	}
}
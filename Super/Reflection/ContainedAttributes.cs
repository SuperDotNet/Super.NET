using System;
using System.Reflection;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Model.Specifications;

namespace Super.Reflection
{
	sealed class ContainedAttributes<TAttribute, T> : Attributes<TAttribute, T> where TAttribute : Attribute
	{
		public static ContainedAttributes<TAttribute, T> Default { get; } = new ContainedAttributes<TAttribute, T>();

		public static ContainedAttributes<TAttribute, T> Inherited { get; }
			= new ContainedAttributes<TAttribute, T>(Declared<TAttribute>.Inherited);

		ContainedAttributes() : this(Declared<TAttribute>.Default) {}

		public ContainedAttributes(IDeclared<TAttribute> attribute)
			: this(attribute, ValueSelector<T>.Default.In(Cast<TAttribute>.Default).ToDelegate()) {}

		public ContainedAttributes(IDeclared<TAttribute> attribute, Func<TAttribute, T> select)
			: this(IsContainedAttribute<TAttribute>.Default, attribute, select) {}

		public ContainedAttributes(ISpecification<ICustomAttributeProvider> specification,
		                           IDeclared<TAttribute> attribute, Func<TAttribute, T> select)
			: base(specification, attribute, select) {}
	}
}
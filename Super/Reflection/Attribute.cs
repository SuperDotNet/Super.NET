using Super.ExtensionMethods;
using Super.Model.Collections;
using Super.Model.Selection;
using Super.Model.Specifications;
using System;
using System.Reflection;

namespace Super.Reflection
{
	class Attribute<TAttribute, T> : Specification<ICustomAttributeProvider, T>, IAttribute<T> where TAttribute : Attribute
	{
		public Attribute(Func<TAttribute, T> select) : this(Attribute<TAttribute>.Default, select) {}

		public Attribute(IAttribute<TAttribute> attribute, Func<TAttribute, T> select)
			: this(IsDefined<TAttribute>.Default, attribute, select) {}

		public Attribute(ISpecification<ICustomAttributeProvider> specification,
		                 IAttribute<TAttribute> attribute, Func<TAttribute, T> select)
			: base(specification, specification.If(attribute.Out(select).ToStore())) {}
	}

	sealed class Attribute<T> : Decorated<ICustomAttributeProvider, T>, IAttribute<T>
	{
		public static Attribute<T> Default { get; } = new Attribute<T>();

		public static Attribute<T> Inherited { get; } = new Attribute<T>(Declared<T>.Inherited);

		Attribute() : this(Declared<T>.Default) {}

		public Attribute(IDeclared<T> declared) : base(declared.Out(SingleSelector<T>.Default)) {}
	}
}
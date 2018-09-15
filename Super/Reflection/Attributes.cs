using Super.Model.Collections;
using Super.Model.Selection;
using Super.Model.Specifications;
using System;
using System.Reflection;

namespace Super.Reflection
{
	class Attributes<TAttribute, T> : Specification<ICustomAttributeProvider, ReadOnlyMemory<T>>, IAttributes<T>
		where TAttribute : Attribute
	{
		public Attributes(Func<TAttribute, T> select) : this(Declared<TAttribute>.Default, select) {}

		public Attributes(IDeclared<TAttribute> attribute, Func<TAttribute, T> select)
			: this(IsDefined<TAttribute>.Default, attribute, select) {}

		public Attributes(ISpecification<ICustomAttributeProvider> specification,
		                  IDeclared<TAttribute> attribute, Func<TAttribute, T> select) // TODO: fix.
			: base(specification, attribute.Select(x => select(x)).ToStore().If(specification)) {}
	}

	sealed class Attributes<T> : ArrayStore<ICustomAttributeProvider, T>, IAttributes<T>
	{
		public static Attributes<T> Default { get; } = new Attributes<T>();

		public static Attributes<T> Inherited { get; } = new Attributes<T>(Declared<T>.Inherited);

		Attributes() : this(Declared<T>.Default) {}

		public Attributes(IDeclared<T> declared) : base(declared.Get) {}
	}
}
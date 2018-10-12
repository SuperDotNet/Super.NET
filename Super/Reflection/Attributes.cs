using Super.Model.Collections;
using Super.Model.Selection;
using Super.Model.Specifications;
using System;
using System.Linq;
using System.Reflection;

namespace Super.Reflection
{
	class Attributes<TAttribute, T> : Specification<ICustomAttributeProvider, Array<T>>, IAttributes<T>
		where TAttribute : Attribute
	{
		public Attributes(Func<TAttribute, T> select) : this(Declared<TAttribute>.Default, select) {}

		public Attributes(IDeclared<TAttribute> attribute, Func<TAttribute, T> select)
			: this(IsDefined<TAttribute>.Default, attribute, select) {}

		public Attributes(ISpecification<ICustomAttributeProvider> specification,
		                  IDeclared<TAttribute> attribute, Func<TAttribute, T> select)
			: base(specification, attribute.Reference()
			                               .Select(x => x.Select(select)) // TODO: fix.
			                               .ToStore()
			                               .If(specification)
			                               .Result()) {}
	}

	sealed class Attributes<T> : ArrayStore<ICustomAttributeProvider, T>, IAttributes<T>
	{
		public static Attributes<T> Default { get; } = new Attributes<T>();

		public static Attributes<T> Inherited { get; } = new Attributes<T>(Declared<T>.Inherited);

		Attributes() : this(Declared<T>.Default) {}

		public Attributes(IDeclared<T> declared) : base(declared.Get) {}
	}
}
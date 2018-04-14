using Super.ExtensionMethods;
using Super.Model.Collections;
using Super.Model.Selection;
using Super.Model.Sources;
using Super.Model.Specifications;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;

namespace Super.Reflection
{
	public interface IMetadata<out T> : ISelect<ICustomAttributeProvider, T> {}

	sealed class IsContainedAttribute<T> : DecoratedSpecification<ICustomAttributeProvider> where T : Attribute
	{
		public static IsContainedAttribute<T> Default { get; } = new IsContainedAttribute<T>();

		IsContainedAttribute()
			: base(IsDefined<T>.Default
			                                .And(IsAssignableFrom<ISource<T>>.Default.Select(Type<T>.Metadata))
			                                .Select(Cast<object>.Default)) {}
	}

	sealed class ContainedAttribute<TAttribute, T> : Attribute<TAttribute, T> where TAttribute : Attribute
	{
		public static ContainedAttribute<TAttribute, T> Default { get; } = new ContainedAttribute<TAttribute, T>();

		public static ContainedAttribute<TAttribute, T> Inherited { get; }
			= new ContainedAttribute<TAttribute, T>(Attribute<TAttribute>.Inherited);

		ContainedAttribute() : this(Attribute<TAttribute>.Default) {}

		public ContainedAttribute(IMetadata<TAttribute> metadata)
			: this(metadata, ValueSelector<T>.Default.In(Cast<TAttribute>.Default).ToDelegate()) {}

		public ContainedAttribute(IMetadata<TAttribute> metadata, Func<TAttribute, T> @select)
			: this(IsContainedAttribute<TAttribute>.Default, metadata, @select) {}

		public ContainedAttribute(ISpecification<ICustomAttributeProvider> specification,
		                          IMetadata<TAttribute> metadata, Func<TAttribute, T> @select) : base(specification, metadata, @select) {}
	}

	class Attribute<TAttribute, T> : Specification<ICustomAttributeProvider, T>, IMetadata<T> where TAttribute : Attribute
	{
		public Attribute(Func<TAttribute, T> select) : this(Attribute<TAttribute>.Default, @select) {}

		public Attribute(IMetadata<TAttribute> metadata, Func<TAttribute, T> select)
			: this(IsDefined<TAttribute>.Default, metadata, select) {}

		public Attribute(ISpecification<ICustomAttributeProvider> specification,
		                 IMetadata<TAttribute> metadata, Func<TAttribute, T> select)
			: base(specification, specification.If(metadata.Out(select).ToStore())) {}
	}

	sealed class Attribute<T> : Decorated<ICustomAttributeProvider, T>, IMetadata<T>
	{
		public static Attribute<T> Default { get; } = new Attribute<T>();

		public static Attribute<T> Inherited { get; } = new Attribute<T>(Declared<T>.Inherited);

		Attribute() : this(Declared<T>.Default) {}

		public Attribute(IDeclared<T> declared) : base(declared.Out(SingleSelector<T>.Default)) {}
	}

	sealed class Attributes<T> : Decorated<ICustomAttributeProvider, ImmutableArray<T>>, IMetadata<ImmutableArray<T>>
	{
		public static Attributes<T> Default { get; } = new Attributes<T>();

		public static Attributes<T> Inherited { get; } = new Attributes<T>(Declared<T>.Inherited);

		Attributes() : this(Declared<T>.Default) {}

		public Attributes(IDeclared<T> declared) : base(declared.Out(Set<T>.Enumerate)) {}
	}

	public interface IDeclared<out T> : IMetadata<IEnumerable<T>> {}

	sealed class Declared<T> : IDeclared<T>
	{
		public static Declared<T> Default { get; } = new Declared<T>();

		public static Declared<T> Inherited { get; } = new Declared<T>(true);

		Declared() : this(false) {}

		readonly Type _type;
		readonly bool _inherit;

		public Declared(bool inherit) : this(Type<T>.Instance, inherit) {}

		public Declared(Type type, bool inherit)
		{
			_type    = type;
			_inherit = inherit;
		}

		public IEnumerable<T> Get(ICustomAttributeProvider parameter)
			=> parameter.GetCustomAttributes(_type, _inherit).Cast<T>();
	}
}
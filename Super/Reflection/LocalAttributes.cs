using Super.Model.Selection;
using Super.Model.Selection.Conditions;
using Super.Model.Sequences;
using System;
using System.Reflection;

namespace Super.Reflection
{
	sealed class LocalAttributes<T> : AttributesStore<T> where T : Attribute
	{
		public static LocalAttributes<T> Default { get; } = new LocalAttributes<T>();

		LocalAttributes() : base(IsDefined<T>.Default, ProvidedAttributes<T>.Default) {}
	}

	sealed class Attributes<T> : AttributesStore<T> where T : Attribute
	{
		public static Attributes<T> Default { get; } = new Attributes<T>();

		Attributes() : base(IsDefined<T>.Inherited, ProvidedAttributes<T>.Inherited) {}
	}

	class AttributesStore<T> : Conditional<ICustomAttributeProvider, Array<T>>, IAttributes<T> where T : Attribute
	{
		public AttributesStore(ICondition<ICustomAttributeProvider> condition,
		                       ISelect<ICustomAttributeProvider, Array<T>> source)
			: base(condition, source.ToTable()) {}
	}

	sealed class LocalAttribute<T> : AttributeStore<T> where T : Attribute
	{
		public static LocalAttribute<T> Default { get; } = new LocalAttribute<T>();

		LocalAttribute() : base(LocalAttributes<T>.Default) {}
	}

	sealed class Attribute<T> : AttributeStore<T> where T : Attribute
	{
		public static Attribute<T> Default { get; } = new Attribute<T>();

		Attribute() : base(Attributes<T>.Default) {}
	}

	class AttributeStore<T> : Conditional<ICustomAttributeProvider, T>, IAttribute<T> where T : Attribute
	{
		public AttributeStore(IAttributes<T> attributes)
			: base(attributes.Condition, attributes.Query().Only().ToTable()) {}
	}
}
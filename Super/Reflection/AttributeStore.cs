using System;
using System.Reflection;
using Super.Model.Selection.Conditions;

namespace Super.Reflection
{
	class AttributeStore<T> : Conditional<ICustomAttributeProvider, T>, IAttribute<T> where T : Attribute
	{
		public AttributeStore(IAttributes<T> attributes)
			: base(attributes.Condition, attributes.Query().Only().ToTable()) {}
	}
}
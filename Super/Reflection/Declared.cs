using System;
using System.Reflection;
using Super.Model.Selection.Conditions;

namespace Super.Reflection
{
	class Declared<TAttribute, T> : Conditional<ICustomAttributeProvider, T> where TAttribute : Attribute
	{
		protected Declared(Func<TAttribute, T> select) : this(LocalAttribute<TAttribute>.Default, select) {}

		protected Declared(IAttribute<TAttribute> attribute, Func<TAttribute, T> select)
			: base(attribute.Condition, attribute.Select(select).ToTable()) {}
	}
}
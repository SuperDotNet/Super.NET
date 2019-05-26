using System;
using System.Reflection;
using Super.Model.Selection;
using Super.Model.Selection.Conditions;
using Super.Model.Sequences;

namespace Super.Reflection
{
	class AttributesStore<T> : Conditional<ICustomAttributeProvider, Array<T>>, IAttributes<T> where T : Attribute
	{
		public AttributesStore(ICondition<ICustomAttributeProvider> condition,
		                       ISelect<ICustomAttributeProvider, Array<T>> source)
			: base(condition, source.ToTable()) {}
	}
}
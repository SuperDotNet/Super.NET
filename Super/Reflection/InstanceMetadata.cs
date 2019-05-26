using System;
using System.Reflection;
using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Selection.Conditions;

namespace Super.Reflection
{
	class InstanceMetadata<TIn, TAttribute, TOut> : Conditional<TIn, TOut> where TAttribute : Attribute, IResult<TOut>
	{
		protected InstanceMetadata() : this(AttributeProvider<TIn>.Default, DeclaredValue<TAttribute, TOut>.Default) {}

		protected InstanceMetadata(ISelect<TIn, ICustomAttributeProvider> select,
		                           IConditional<ICustomAttributeProvider, TOut> value)
			: base(select.Select(value.Condition).Get, select.Select(value.Get).Get) {}
	}
}
using Super.Compose;
using Super.Model.Results;
using Super.Model.Selection;
using Super.Model.Selection.Conditions;
using System;
using System.Reflection;

namespace Super.Reflection
{
	class InstanceMetadata<TIn, TAttribute, TOut> : Conditional<TIn, TOut> where TAttribute : Attribute, IResult<TOut>
	{
		protected InstanceMetadata() : this(AttributeProvider<TIn>.Default, DeclaredValue<TAttribute, TOut>.Default) {}

		protected InstanceMetadata(ISelect<TIn, ICustomAttributeProvider> select,
		                           IConditional<ICustomAttributeProvider, TOut> value)
			: base(select.Select(value.Condition).Get, select.Select(value.Get).Get) {}
	}

	sealed class AttributeProvider<T> : DecoratedSelect<T, ICustomAttributeProvider>
	{
		public static AttributeProvider<T> Default { get; } = new AttributeProvider<T>();

		AttributeProvider() : base(Start.A.Selection<T>()
		                                .By.Metadata.UnlessOf(Start.A.Selection<ICustomAttributeProvider>().By.Self)) {}
	}
}
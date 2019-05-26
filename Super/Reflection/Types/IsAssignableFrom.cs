using Super.Compose;
using Super.Model.Selection.Conditions;
using Super.Model.Sequences;
using Super.Runtime.Activation;
using System;
using System.Reflection;

namespace Super.Reflection.Types
{
	public sealed class IsAssignableFrom<T> : Condition<Type>
	{
		public static IsAssignableFrom<T> Default { get; } = new IsAssignableFrom<T>();

		IsAssignableFrom() : base(new IsAssignableFrom(A.Metadata<T>())) {}
	}

	public sealed class IsAssignableFrom : Condition<Type>, IActivateUsing<Type>
	{
		public IsAssignableFrom(Type type) : base(type.IsAssignableFrom) {}
	}

	public sealed class ContainsGenericInterface : ICondition<TypeInfo>
	{
		readonly static Func<TypeInfo, IConditional<Type, Array<Type>>>
			Implementations = GenericInterfaceImplementations.Default.Get;

		readonly Type                                                _definition;
		readonly Func<TypeInfo, IConditional<Type, Array<Type>>> _implementations;

		public ContainsGenericInterface(Type definition) : this(definition, Implementations) {}

		public ContainsGenericInterface(Type definition,
		                                Func<TypeInfo, IConditional<Type, Array<Type>>> implementations)
		{
			_definition      = definition;
			_implementations = implementations;
		}

		public bool Get(TypeInfo parameter) => _implementations(parameter).Condition.Get(_definition);
	}
}
using System;
using System.Reflection;
using Super.Model.Selection.Conditions;
using Super.Model.Sequences;

namespace Super.Reflection.Types
{
	public sealed class ContainsGenericInterface : ICondition<TypeInfo>
	{
		readonly static Func<TypeInfo, IConditional<Type, Array<Type>>>
			Implementations = GenericInterfaceImplementations.Default.Get;

		readonly Type                                            _definition;
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
using Super.Compose;
using Super.Model.Selection;
using Super.Model.Selection.Conditions;
using Super.Model.Selection.Stores;
using Super.Model.Sequences;
using System;
using System.Reflection;

namespace Super.Reflection.Types
{
	sealed class GenericInterfaceImplementations : Store<Type, IConditional<Type, Array<TypeInfo>>>
	{
		public static GenericInterfaceImplementations Default { get; } = new GenericInterfaceImplementations();

		GenericInterfaceImplementations() : this(GenericTypeDefinition.Default) {}

		public GenericInterfaceImplementations(ISelect<Type, Type> definition)
			: base(GenericInterfaces.Default
			                        .Grouping(definition)
			                        .Then()
			                        .Activate<Model.Sequences.Query.Lookup<Type, TypeInfo>>()
			                        .Get()
			                        .Select(definition.Unless(IsGenericTypeDefinition.Default, A.Self<Type>()))
			                        .Get) {}
	}
}
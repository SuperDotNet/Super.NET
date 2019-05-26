using System;
using Super.Compose;
using Super.Model.Selection;
using Super.Model.Selection.Conditions;
using Super.Model.Selection.Stores;
using Super.Model.Sequences;

namespace Super.Reflection.Types
{
	sealed class GenericInterfaceImplementations : Store<Type, IConditional<Type, Array<Type>>>
	{
		public static GenericInterfaceImplementations Default { get; } = new GenericInterfaceImplementations();

		GenericInterfaceImplementations() : this(GenericTypeDefinition.Default) {}

		public GenericInterfaceImplementations(ISelect<Type, Type> definition)
			: base(GenericInterfaces.Default.Query()
			                        .GroupMap(definition.ToDelegate())
			                        .Select(definition.Unless(IsGenericTypeDefinition.Default, A.Self<Type>()).Select)
			                        .Get) {}
	}
}
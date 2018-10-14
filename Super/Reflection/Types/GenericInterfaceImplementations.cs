using Super.Model.Selection;
using Super.Model.Selection.Stores;
using Super.Model.Sequences;
using System;
using System.Reflection;

namespace Super.Reflection.Types
{
	sealed class GenericInterfaceImplementations : Store<TypeInfo, ISpecification<Type, Array<TypeInfo>>>
	{
		public static GenericInterfaceImplementations Default { get; } = new GenericInterfaceImplementations();

		GenericInterfaceImplementations() : this(GenericTypeDefinition.Default) {}

		public GenericInterfaceImplementations(ISelect<Type, Type> definition)
			: base(GenericInterfaces.Default.Grouping(definition)
			                        .Select(I<Model.Sequences.Query.Lookup<Type, TypeInfo>>.Default)
			                        .Select(definition.Unless(IsGenericTypeDefinition.Default, Self<Type>.Default))
			                        .Get) {}
	}
}
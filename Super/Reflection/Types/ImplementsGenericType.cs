using Super.Model.Specifications;
using System;
using System.Reflection;

namespace Super.Reflection.Types
{
	public class ImplementsGenericType : FixedParameterSelectedSpecification<TypeInfo, TypeInfo>
	{
		readonly static Func<TypeInfo, Func<TypeInfo, bool>> Select
			= Implementations.GenericInterfaceImplementations.Select(x => x.AsSpecification().ToDelegate()).Get;

		public ImplementsGenericType(TypeInfo definition) : this(definition, Select) {}

		public ImplementsGenericType(TypeInfo definition, Func<TypeInfo, Func<TypeInfo, bool>> select)
			: base(select, definition) {}
	}
}
using Super.Compose;
using Super.Model.Selection.Conditions;
using System;
using System.Reflection;

namespace Super.Reflection.Types
{
	public class ImplementsGenericType : Condition<TypeInfo>
	{
		public ImplementsGenericType(Type definition)
			: base(Start.An.Instance(GenericInterfaceImplementations.Default)
			            .Select(x => x.Condition)
			            .Select(definition.To)
			            .Then()) {}
	}
}
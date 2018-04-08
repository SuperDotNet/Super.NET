using Super.ExtensionMethods;
using Super.Model.Selection;
using Super.Model.Specifications;
using System;
using System.Reflection;

namespace Super.Reflection
{
	public class ImplementsGenericType : DelegatedSpecification<TypeInfo>
	{
		public ImplementsGenericType(Type definition)
			: base(Select.New<TypeInfo, HasGenericInterface>()
			             .Out(new FixedParameterSpecification<TypeInfo>(definition.GetTypeInfo()).ToDelegate())
			             .Get) {}
	}
}
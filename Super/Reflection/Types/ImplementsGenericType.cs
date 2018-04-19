using System;
using System.Reflection;
using Super.Model.Selection;
using Super.Model.Specifications;

namespace Super.Reflection.Types
{
	public class ImplementsGenericType : DelegatedSpecification<TypeInfo>
	{
		public ImplementsGenericType(Type definition)
			: base(Select.New<TypeInfo, HasGenericInterface>()
			             .Out(new FixedParameterSpecification<TypeInfo>(definition.GetTypeInfo()).ToDelegate())
			             .Get) {}
	}
}
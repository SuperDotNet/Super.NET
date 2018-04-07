using Super.ExtensionMethods;
using Super.Model.Specifications;
using Super.Runtime.Activation;
using System;
using System.Reflection;

namespace Super.Reflection
{
	public class ImplementsGenericType : DelegatedSpecification<TypeInfo>
	{
		public ImplementsGenericType(Type definition)
			: base(Activate.New<TypeInfo, HasGenericInterface>()
			               .Out(new FixedParameterSpecification<TypeInfo>(definition.GetTypeInfo()).ToDelegate())
			               .Get) {}
	}
}
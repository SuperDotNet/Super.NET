using Super.Model.Specifications;
using System;
using System.Reflection;

namespace Super.Reflection.Types
{
	public class ImplementsGenericType : DecoratedSpecification<TypeInfo>
	{
		public ImplementsGenericType(Type definition)
			: base(In<TypeInfo>.Out<HasGenericInterface>()
			                   .Enter(new FixedParameterSpecification<TypeInfo>(definition.GetTypeInfo()))) {}
	}
}
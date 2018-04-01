using System;
using System.Reflection;
using Super.ExtensionMethods;
using Super.Model.Specifications;
using Super.Runtime.Invocation;

namespace Super.Reflection
{
	public class ImplementsGenericType : DelegatedSpecification<TypeInfo>
	{
		public ImplementsGenericType(Type definition)
			: base(I<HasGenericInterface>.Default.Infer(default(TypeInfo))
			                             .Adapt()
			                             .Out(new FixedParameterCoercer<TypeInfo, bool>(definition.GetTypeInfo()))
			                             .Get) {}
	}
}
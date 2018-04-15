using System;
using System.Reflection;
using Super.ExtensionMethods;
using Super.Model.Collections;
using Super.Model.Specifications;

namespace Super.Reflection.Types
{
	sealed class HasGenericArguments : AllSpecification<TypeInfo>
	{
		public static HasGenericArguments Default { get; } = new HasGenericArguments();

		HasGenericArguments() : base(IsGenericType.Default, HasAny<Type>.Default.Select(GenericArgumentsSelector.Default)) {}
	}
}
using Super.Model.Specifications;
using System.Reflection;

namespace Super.Reflection.Types
{
	sealed class HasGenericArguments : AllSpecification<TypeInfo>
	{
		public static HasGenericArguments Default { get; } = new HasGenericArguments();

		HasGenericArguments() : base(IsGenericType.Default,
		                             GenericArgumentsSelector.Default.HasAny().Return()) {}
	}
}
using Super.Model.Selection.Conditions;
using System.Reflection;

namespace Super.Reflection.Types
{
	sealed class HasGenericArguments : AllCondition<TypeInfo>
	{
		public static HasGenericArguments Default { get; } = new HasGenericArguments();

		HasGenericArguments() : base(IsGenericType.Default.ToDelegate(),
		                             GenericArgumentsSelector.Default.Then().HasAny()) {}
	}
}
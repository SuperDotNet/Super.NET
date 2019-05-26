using System.Reflection;
using Super.Model.Selection.Conditions;

namespace Super.Reflection.Types
{
	sealed class HasGenericArguments : AllCondition<TypeInfo>
	{
		public static HasGenericArguments Default { get; } = new HasGenericArguments();

		HasGenericArguments() : base(IsGenericType.Default.ToDelegate(),
		                             GenericArguments.Default.Open().Then().HasAny()) {}
	}
}
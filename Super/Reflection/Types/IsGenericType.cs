using System.Reflection;
using Super.Model.Selection.Conditions;

namespace Super.Reflection.Types
{
	sealed class IsGenericType : DelegatedCondition<TypeInfo>
	{
		public static IsGenericType Default { get; } = new IsGenericType();

		IsGenericType() : base(x => x.IsGenericType) {}
	}
}
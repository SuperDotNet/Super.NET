using System.Reflection;
using Super.Model.Selection.Conditions;

namespace Super.Reflection.Types
{
	sealed class IsValueType : ICondition<TypeInfo>
	{
		public static IsValueType Default { get; } = new IsValueType();

		IsValueType() {}

		public bool Get(TypeInfo parameter) => parameter.IsValueType;
	}
}
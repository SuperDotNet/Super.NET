using System.Reflection;
using Super.Model.Selection.Conditions;
using Super.Reflection.Types;

namespace Super.Reflection
{
	sealed class IsReference : InverseCondition<TypeInfo>
	{
		public static IsReference Default { get; } = new IsReference();

		IsReference() : base(IsValueType.Default) {}
	}
}
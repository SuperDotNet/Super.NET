using System;
using System.Reflection;
using Super.Model.Selection.Alterations;

namespace Super.Reflection
{
	sealed class AccountForUnassignedType : IAlteration<TypeInfo>
	{
		public static AccountForUnassignedType Default { get; } = new AccountForUnassignedType();

		AccountForUnassignedType() {}

		public TypeInfo Get(TypeInfo parameter) => Nullable.GetUnderlyingType(parameter.AsType())
		                                                   ?.GetTypeInfo() ?? parameter;
	}
}
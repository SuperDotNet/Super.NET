using System;
using System.Reflection;
using Super.Model.Sources.Alterations;

namespace Super.Reflection
{
	sealed class AccountForUnassignedAlteration : IAlteration<TypeInfo>
	{
		public static AccountForUnassignedAlteration Default { get; } = new AccountForUnassignedAlteration();

		AccountForUnassignedAlteration() {}

		public TypeInfo Get(TypeInfo parameter) => Nullable.GetUnderlyingType(parameter.AsType())
		                                                   ?.GetTypeInfo() ?? parameter;
	}
}
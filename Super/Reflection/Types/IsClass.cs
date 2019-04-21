using System;
using Super.Model.Selection.Conditions;

namespace Super.Reflection.Types
{
	sealed class IsClass : DelegatedCondition<Type>
	{
		public static IsClass Default { get; } = new IsClass();

		IsClass() : base(x => x.IsClass) {}
	}
}
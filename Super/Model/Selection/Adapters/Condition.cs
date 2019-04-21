using Super.Model.Selection.Conditions;
using System;

namespace Super.Model.Selection.Adapters
{
	public class Condition<T> : DelegatedCondition<T>
	{
		public static implicit operator Condition<T>(Func<T, bool> value) => new Condition<T>(value);

		public Condition(Func<T, bool> @delegate) : base(@delegate) {}
	}

	public class Condition : DelegatedResultCondition
	{
		public static implicit operator Condition(Func<bool> value) => new Condition(value);

		public Condition(Func<bool> @delegate) : base(@delegate) {}
	}
}

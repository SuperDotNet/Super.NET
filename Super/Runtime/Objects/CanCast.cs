using Super.Compose;
using Super.Model.Selection.Conditions;
using Super.Reflection.Types;

namespace Super.Runtime.Objects
{
	public sealed class CanCast<TFrom, TTo> : AllCondition<TFrom>
	{
		public static CanCast<TFrom, TTo> Default { get; } = new CanCast<TFrom, TTo>();

		CanCast() : base(Start.A.Condition<TFrom>().By.Assigned,
		                 Start.A.Selection<TFrom>()
		                      .By.Metadata.Select(IsAssignableFrom<TTo>.Default)) {}
	}
}
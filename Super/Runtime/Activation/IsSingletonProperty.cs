using System.Reflection;
using Super.Compose;
using Super.Model.Selection.Conditions;

namespace Super.Runtime.Activation
{
	sealed class IsSingletonProperty : AllCondition<PropertyInfo>
	{
		public static IsSingletonProperty Default { get; } = new IsSingletonProperty();

		IsSingletonProperty() : base(Start.A.Condition.Of.Any.By.Assigned,
		                             Start.A.Condition<PropertyInfo>()
		                                  .By.Calling(y => y.CanRead && y.GetMethod.IsStatic)) {}
	}
}
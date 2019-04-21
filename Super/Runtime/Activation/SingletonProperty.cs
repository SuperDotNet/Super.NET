using Super.Compose;
using Super.Model.Results;
using Super.Model.Selection.Conditions;
using Super.Model.Selection.Stores;
using Super.Model.Sequences;
using System;
using System.Reflection;

namespace Super.Runtime.Activation
{
	sealed class SingletonProperty : ReferenceValueStore<Type, PropertyInfo>
	{
		public static SingletonProperty Default { get; } = new SingletonProperty();

		SingletonProperty() : this(SingletonCandidates.Default) {}

		public SingletonProperty(IResult<Array<string>> candidates)
			: base(Start.A.Selection.Of.System.Type.By.Delegate<string, PropertyInfo>(x => x.GetProperty)
			            .Select(candidates.Select)
			            .Then()
			            .Value()
			            .Query()
			            .Where(IsSingletonProperty.Default)
			            .FirstAssigned()
			            .Get) {}
	}

	sealed class IsSingletonProperty : AllCondition<PropertyInfo>
	{
		public static IsSingletonProperty Default { get; } = new IsSingletonProperty();

		IsSingletonProperty() : base(Start.A.Condition.Of.Any.By.Assigned,
		                             Start.A.Condition<PropertyInfo>()
		                                  .By.Calling(y => y.CanRead && y.GetMethod.IsStatic)) {}
	}
}
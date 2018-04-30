using Super.Model.Collections;
using Super.Model.Selection;
using Super.Model.Sources;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Super.Runtime.Activation
{
	static class Implementations
	{
		public static ISelect<Type, PropertyInfo> SingletonProperty { get; } =
			Activation.SingletonProperty.Default.ToReferenceStore();
	}

	sealed class SingletonProperty : DecoratedSelect<Type, PropertyInfo>
	{
		public static SingletonProperty Default { get; } = new SingletonProperty();

		SingletonProperty() : this(SingletonCandidates.Default) {}

		public SingletonProperty(ISource<IEnumerable<string>> candidates)
			: base(In<Type>.Select(x => new SelectSelector<string, PropertyInfo>(x.GetProperty))
			               .Select(candidates.Select)
			               .Select(x => x.Select(SingletonPropertyPredicate.Default))
			               .Value()
			               .FirstAssigned()) {}
	}

	sealed class SingletonPropertyPredicate : WhereSelector<PropertyInfo>
	{
		public static SingletonPropertyPredicate Default { get; } = new SingletonPropertyPredicate();

		SingletonPropertyPredicate() : base(IsAssigned.Default
		                                              .And(In<PropertyInfo>.Is(y => y.CanRead && y.GetMethod.IsStatic))
		                                              .IsSatisfiedBy) {}
	}
}
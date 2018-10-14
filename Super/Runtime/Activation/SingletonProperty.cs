using Super.Model.Selection;
using Super.Model.Sequences;
using Super.Model.Sequences.Query;
using Super.Model.Sources;
using Super.Model.Specifications;
using System;
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

		SingletonProperty() : this(SingletonCandidates.Default.Out(x => x.Reference().Fixed().View()).Out()) {}

		public SingletonProperty(ISource<ArrayView<string>> candidates)
			: base(In<Type>.Select(x => new ArraySelector<string, PropertyInfo>(x.GetProperty))
			               .Select(candidates.Select)
			               .Value()
			               .Select(x => x.ToArray())
			               .Sequence()
			               .Where(IsSingletonProperty.Default.IsSatisfiedBy)
			               .FirstAssigned()) {}
	}

	sealed class IsSingletonProperty : AllSpecification<PropertyInfo>
	{
		public static IsSingletonProperty Default { get; } = new IsSingletonProperty();

		IsSingletonProperty() : base(IsAssigned.Default,
		                             In<PropertyInfo>.Is(y => y.CanRead && y.GetMethod.IsStatic)) {}
	}
}
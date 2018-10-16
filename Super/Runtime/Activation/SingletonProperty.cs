using Super.Model.Selection.Stores;
using Super.Model.Sequences;
using Super.Model.Sequences.Query;
using Super.Model.Sources;
using Super.Model.Specifications;
using System;
using System.Reflection;

namespace Super.Runtime.Activation
{
	sealed class SingletonProperty : ReferenceValueTable<Type, PropertyInfo>
	{
		public static SingletonProperty Default { get; } = new SingletonProperty();

		SingletonProperty() : this(SingletonCandidates.Default.Out(x => x.Reference().Fixed().View().Out())) {}

		public SingletonProperty(ISource<ArrayView<string>> candidates)
			: base(Start.From<Type>()
			            .Delegate<string, PropertyInfo>(x => x.GetProperty)
			            .Select(x => new ArraySelector<string, PropertyInfo>(x))
			            .Select(candidates.Select)
			            .Value()
			            .Select(x => x.ToArray())
			            .Sequence()
			            .Where(IsSingletonProperty.Default.IsSatisfiedBy)
			            .FirstAssigned()
			            .Get) {}
	}

	sealed class IsSingletonProperty : AllSpecification<PropertyInfo>
	{
		public static IsSingletonProperty Default { get; } = new IsSingletonProperty();

		IsSingletonProperty() : base(IsAssigned.Default,
		                             Start.When<PropertyInfo>().Is(y => y.CanRead && y.GetMethod.IsStatic)) {}
	}
}
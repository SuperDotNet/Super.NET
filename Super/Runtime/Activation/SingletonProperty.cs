using Super.Model.Selection;
using System;
using System.Linq;
using System.Reflection;

namespace Super.Runtime.Activation
{
	static class Implementations
	{
		public static ISelect<Type, PropertyInfo> SingletonProperties { get; } = SingletonProperty.Default.ToReferenceStore();
	}

	sealed class SingletonProperty : ISelect<Type, PropertyInfo>
	{
		public static SingletonProperty Default { get; } = new SingletonProperty();

		SingletonProperty() : this(SingletonCandidates.Default) {}

		readonly ISingletonCandidates _candidates;

		public SingletonProperty(ISingletonCandidates candidates) => _candidates = candidates;

		public PropertyInfo Get(Type parameter) => _candidates.Select(parameter.GetProperty)
		                                                      .Assigned()
		                                                      .FirstOrDefault(x => x.CanRead && x.GetMethod.IsStatic);
	}
}
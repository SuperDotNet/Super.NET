using Super.Model.Selection;
using System;
using System.Linq;
using System.Reflection;

namespace Super.Runtime.Activation
{
	sealed class SingletonProperty : ISelect<Type, PropertyInfo>
	{
		public static ISelect<Type, PropertyInfo> Default { get; } = new SingletonProperty().ToReferenceStore();

		SingletonProperty() : this(SingletonCandidates.Default) {}

		readonly ISingletonCandidates _candidates;

		public SingletonProperty(ISingletonCandidates candidates) => _candidates = candidates;

		public PropertyInfo Get(Type parameter) => _candidates.Select(parameter.GetProperty)
		                                                      .Assigned()
		                                                      .FirstOrDefault(x => x.CanRead && x.GetMethod.IsStatic);
	}
}
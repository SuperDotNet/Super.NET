using Super.Model.Instances;
using Super.Model.Sources;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace Super.Runtime.Environment
{
	sealed class ComponentTypeLocator : ISource<Type, Type>
	{
		public static ComponentTypeLocator Default { get; } = new ComponentTypeLocator();

		ComponentTypeLocator() : this(ComponentTypeCandidates.Default) {}

		readonly ImmutableArray<Type> _candidates;

		public ComponentTypeLocator(ImmutableArray<Type> candidates) => _candidates = candidates;

		public Type Get(Type parameter) => _candidates.FirstOrDefault(parameter.IsAssignableFrom) ??
		                                   _candidates.FirstOrDefault(typeof(IInstance<>)
		                                                              .MakeGenericType(parameter)
		                                                              .IsAssignableFrom);
	}
}
using System.Collections.Immutable;
using System.Reflection;
using Super.ExtensionMethods;
using Super.Model.Sources;
using Super.Runtime.Activation;

namespace Super.Reflection
{
	public sealed class GenericInterfaceImplementations
		: ISource<TypeInfo, ImmutableArray<TypeInfo>>, IActivateMarker<TypeInfo>
	{
		readonly ImmutableArray<TypeInfo> _candidates;

		public GenericInterfaceImplementations(TypeInfo type) :
			this(GenericInterfaces.Default.Get(type).ToImmutableArray()) {}

		public GenericInterfaceImplementations(ImmutableArray<TypeInfo> candidates) => _candidates = candidates;

		public ImmutableArray<TypeInfo> Get(TypeInfo parameter)
			=> _candidates.Introduce(parameter.GetGenericTypeDefinition(),
			                         t => t.Item1.GetGenericTypeDefinition() == t.Item2)
			              .ToImmutableArray();
	}
}
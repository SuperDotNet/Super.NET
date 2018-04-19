using Super.Model.Selection;
using Super.Runtime.Activation;
using System.Collections.Immutable;
using System.Reflection;

namespace Super.Reflection.Types
{
	public sealed class GenericInterfaceImplementations
		: ISelect<TypeInfo, ImmutableArray<TypeInfo>>, IActivateMarker<TypeInfo>
	{
		readonly ImmutableArray<TypeInfo> _candidates;

		public GenericInterfaceImplementations(TypeInfo type) :
			this(GenericInterfaces.Default.Get(type).ToImmutableArray()) {}

		public GenericInterfaceImplementations(ImmutableArray<TypeInfo> candidates) => _candidates = candidates;

		public ImmutableArray<TypeInfo> Get(TypeInfo parameter)
			=> _candidates.AsEnumerable()
			              .Introduce(parameter.GetGenericTypeDefinition(), t => t.Item1.GetGenericTypeDefinition() == t.Item2)
			              .ToImmutableArray();
	}
}
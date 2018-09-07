using JetBrains.Annotations;
using Super.Model.Collections;
using Super.Runtime.Activation;
using System.Collections.Generic;
using System.Reflection;

namespace Super.Reflection.Types
{
	public sealed class GenericInterfaceImplementations : ArrayStore<TypeInfo, TypeInfo>, IActivateMarker<TypeInfo>
	{
		public GenericInterfaceImplementations(TypeInfo type) : base(I<Sequence>.Default.From(type).ToArray().Get) {}

		sealed class Sequence : ISequence<TypeInfo, TypeInfo>, IActivateMarker<TypeInfo>
		{
			readonly IEnumerable<TypeInfo> _candidates;

			[UsedImplicitly]
			public Sequence(TypeInfo type) : this(Implementations.GenericInterfaces.Get(type).AsEnumerable()) {}

			public Sequence(IEnumerable<TypeInfo> candidates) => _candidates = candidates;

			public IEnumerable<TypeInfo> Get(TypeInfo parameter)
				=> _candidates.Introduce(parameter.GetGenericTypeDefinition(),
				                         t => t.Item1.GetGenericTypeDefinition() == t.Item2);
		}
	}
}
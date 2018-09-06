using JetBrains.Annotations;
using Super.Model.Collections;
using Super.Runtime.Activation;
using System.Collections.Generic;
using System.Reflection;

namespace Super.Reflection.Types
{
	public sealed class GenericInterfaceImplementations : SequenceStore<TypeInfo, TypeInfo>, IActivateMarker<TypeInfo>
	{
		public GenericInterfaceImplementations(TypeInfo type) : base(I<Stream>.Default.From(type)) {}

		sealed class Stream : IStream<TypeInfo, TypeInfo>, IActivateMarker<TypeInfo>
		{
			readonly IEnumerable<TypeInfo> _candidates;

			[UsedImplicitly]
			public Stream(TypeInfo type) : this(Implementations.GenericInterfaces.Get(type).AsEnumerable()) {}

			public Stream(IEnumerable<TypeInfo> candidates) => _candidates = candidates;

			public IEnumerable<TypeInfo> Get(TypeInfo parameter)
				=> _candidates.Introduce(parameter.GetGenericTypeDefinition(),
				                         t => t.Item1.GetGenericTypeDefinition() == t.Item2);
		}
	}
}
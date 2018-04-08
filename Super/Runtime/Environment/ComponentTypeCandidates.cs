using Super.Model.Specifications;
using Super.Runtime.Activation;
using System;
using System.Collections.Immutable;
using System.Linq;
using Super.Model.Sources;
using Super.Reflection.Selection;

namespace Super.Runtime.Environment
{
	sealed class ComponentTypeCandidates : Source<ImmutableArray<Type>>
	{
		public static ComponentTypeCandidates Default { get; } = new ComponentTypeCandidates();

		ComponentTypeCandidates() : base(ComponentAssemblies.Default
		                                                    .SelectMany(Activate<PublicAssemblyTypes>.New)
		                                                    .Where(CanActivate.Default.IsSatisfiedBy)
		                                                    .ToImmutableArray()) {}
	}
}
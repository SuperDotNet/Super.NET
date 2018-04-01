using Super.Model.Instances;
using Super.Model.Specifications;
using Super.Reflection.Query;
using Super.Runtime.Activation;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace Super.Runtime.Environment
{
	sealed class ComponentTypeCandidates : Instance<ImmutableArray<Type>>
	{
		public static ComponentTypeCandidates Default { get; } = new ComponentTypeCandidates();

		ComponentTypeCandidates() : base(ComponentAssemblies.Default.SelectMany(Activation.Locator<PublicAssemblyTypes>.New)
		                                                    .Where(CanActivate.Default.IsSatisfiedBy)
		                                                    .ToImmutableArray()) {}
	}
}
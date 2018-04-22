using Super.Model.Sources;
using Super.Model.Specifications;
using Super.Reflection.Selection;
using Super.Runtime.Activation;
using Super.Runtime.Execution;
using System;
using System.Collections.Immutable;
using System.Linq;

namespace Super.Runtime.Environment
{
	public interface ITypeCandidates : ISource<ImmutableArray<Type>> {}

	sealed class ComponentTypeCandidates : Ambient<ImmutableArray<Type>>, ITypeCandidates
	{
		public static ComponentTypeCandidates Default { get; } = new ComponentTypeCandidates();

		ComponentTypeCandidates() : this(DefaultComponentTypeCandidates.Default, ComponentTypeCandidatesStore.Default) {}

		public ComponentTypeCandidates(ITypeCandidates source, IMutable<ImmutableArray<Type>> mutable) : base(source, mutable) {}
	}

	sealed class DefaultComponentTypeCandidates : Source<ImmutableArray<Type>>, ITypeCandidates
	{
		public static DefaultComponentTypeCandidates Default { get; } = new DefaultComponentTypeCandidates();

		DefaultComponentTypeCandidates() : base(ComponentAssemblies.Default
		                                                           .SelectMany(Activate<PublicAssemblyTypes>.New)
		                                                           .Where(CanActivate.Default.IsSatisfiedBy)
		                                                           .ToImmutableArray()) {}
	}

	public sealed class ComponentTypeCandidatesStore : Logical<ImmutableArray<Type>>
	{
		public static ComponentTypeCandidatesStore Default { get; } = new ComponentTypeCandidatesStore();

		ComponentTypeCandidatesStore() {}
	}
}
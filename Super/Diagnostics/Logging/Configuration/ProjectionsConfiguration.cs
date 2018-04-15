using System;
using System.Collections.Immutable;
using System.Linq;
using Super.ExtensionMethods;
using Super.Runtime.Objects;

namespace Super.Diagnostics.Logging.Configuration
{
	public sealed class ProjectionsConfiguration : LoggerConfigurations
	{
		public static ProjectionsConfiguration Default { get; } = new ProjectionsConfiguration();

		ProjectionsConfiguration() : this(Projectors.Default,
		                                  KnownProjectors.Default.Select(x => x.Key).ToImmutableArray()) {}

		public ProjectionsConfiguration(IProjectors projectors, ImmutableArray<Type> projectionTypes)
			: base(new EnrichmentConfiguration(new ProjectionEnricher(projectors)).ToConfiguration(),
			       new ScalarConfiguration(projectionTypes.AsEnumerable()).ToConfiguration()) {}
	}
}
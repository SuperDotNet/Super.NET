using Super.Runtime.Objects;
using System;
using System.Linq;

namespace Super.Diagnostics.Logging.Configuration
{
	public sealed class ProjectionsConfiguration : LoggerConfigurations
	{
		public static ProjectionsConfiguration Default { get; } = new ProjectionsConfiguration();

		ProjectionsConfiguration() : this(Projectors.Default,
		                                  KnownProjectors.Default.Get().Select(x => x.Key).ToArray()) {}

		public ProjectionsConfiguration(IProjectors projectors, params Type[] projectionTypes)
			: base(new EnrichmentConfiguration(new ProjectionEnricher(projectors)).ToConfiguration(),
			       new ScalarConfiguration(projectionTypes).ToConfiguration()) {}
	}
}
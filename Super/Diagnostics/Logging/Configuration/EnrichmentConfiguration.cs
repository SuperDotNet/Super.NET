using System.Collections.Immutable;
using System.Linq;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;

namespace Super.Diagnostics.Logging.Configuration
{
	sealed class EnrichmentConfiguration : ILoggingEnrichmentConfiguration
	{
		readonly ImmutableArray<ILogEventEnricher> _enrichers;

		public EnrichmentConfiguration(params ILogEventEnricher[] enrichers) : this(enrichers.ToImmutableArray()) {}

		public EnrichmentConfiguration(ImmutableArray<ILogEventEnricher> enrichers) => _enrichers = enrichers;

		public LoggerConfiguration Get(LoggerEnrichmentConfiguration parameter) => parameter.With(_enrichers.ToArray());
	}
}
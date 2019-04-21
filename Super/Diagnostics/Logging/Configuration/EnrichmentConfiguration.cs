using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using System.Collections.Immutable;
using System.Linq;

namespace Super.Diagnostics.Logging.Configuration
{
	public class EnrichmentConfiguration : ILoggingEnrichmentConfiguration
	{
		readonly ImmutableArray<ILogEventEnricher> _enrichers;

		public EnrichmentConfiguration(params ILogEventEnricher[] enrichers) : this(enrichers.ToImmutableArray()) {}

		public EnrichmentConfiguration(ImmutableArray<ILogEventEnricher> enrichers) => _enrichers = enrichers;

		public LoggerConfiguration Get(LoggerEnrichmentConfiguration parameter)
			=> parameter.With(_enrichers.ToArray());
	}
}
using Serilog;
using Serilog.Configuration;
using Serilog.Enrichers.Correlation;
using Super.Diagnostics.Logging.Configuration;
using Super.Model.Selection;

namespace Super.Diagnostics.Logging.Enrichment
{
	sealed class CorrelationIdEnricher : Select<LoggerEnrichmentConfiguration, LoggerConfiguration>,
	                                     ILoggingEnrichmentConfiguration
	{
		public static CorrelationIdEnricher Default { get; } = new CorrelationIdEnricher();

		CorrelationIdEnricher() : base(x => x.WithCorrelationId()) {}
	}
}
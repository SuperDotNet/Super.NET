using Serilog;
using Serilog.Configuration;
using Serilog.Enrichers.Correlation;
using Super.Model.Selection;

namespace Super.Diagnostics
{
	sealed class CorrelationIdEnricher : Select<LoggerEnrichmentConfiguration, LoggerConfiguration>,
	                                     ILoggingEnrichmentConfiguration
	{
		public static CorrelationIdEnricher Default { get; } = new CorrelationIdEnricher();

		CorrelationIdEnricher() : base(x => x.WithCorrelationId()) {}
	}
}
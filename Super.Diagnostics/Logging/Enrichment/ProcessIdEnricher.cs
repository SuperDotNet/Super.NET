using Serilog;
using Serilog.Configuration;
using Super.Diagnostics.Logging.Configuration;
using Super.Model.Selection;

namespace Super.Diagnostics.Logging.Enrichment
{
	sealed class ProcessIdEnricher : Select<LoggerEnrichmentConfiguration, LoggerConfiguration>, ILoggingEnrichmentConfiguration
	{
		public static ProcessIdEnricher Default { get; } = new ProcessIdEnricher();

		ProcessIdEnricher() : base(x => x.WithProcessId()) {}
	}
}
using Serilog;
using Serilog.Configuration;
using Super.Diagnostics.Logging.Configuration;
using Super.Model.Selection;

namespace Super.Diagnostics.Logging.Enrichment
{
	sealed class ProcessNameEnricher : Select<LoggerEnrichmentConfiguration, LoggerConfiguration>, ILoggingEnrichmentConfiguration
	{
		public static ProcessNameEnricher Default { get; } = new ProcessNameEnricher();

		ProcessNameEnricher() : base(x => x.WithProcessName()) {}
	}
}
using Serilog;
using Serilog.Configuration;
using Super.Diagnostics.Logging.Configuration;
using Super.Model.Selection;

namespace Super.Diagnostics.Logging.Enrichment
{
	sealed class ThreadEnricher : Select<LoggerEnrichmentConfiguration, LoggerConfiguration>, ILoggingEnrichmentConfiguration
	{
		public static ThreadEnricher Default { get; } = new ThreadEnricher();

		ThreadEnricher() : base(x => x.WithThreadId()) {}
	}
}
using Serilog;
using Serilog.Configuration;
using Super.Diagnostics.Logging.Configuration;
using Super.Model.Selection;

namespace Super.Diagnostics.Logging.Enrichment
{
	sealed class LogContextEnricher : Select<LoggerEnrichmentConfiguration, LoggerConfiguration>, ILoggingEnrichmentConfiguration
	{
		public static LogContextEnricher Default { get; } = new LogContextEnricher();

		LogContextEnricher() : base(x => x.FromLogContext()) {}
	}
}
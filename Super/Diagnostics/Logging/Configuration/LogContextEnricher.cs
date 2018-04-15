using Serilog;
using Serilog.Configuration;
using Super.Model.Selection;

namespace Super.Diagnostics.Logging.Configuration
{
	sealed class LogContextEnricher : Delegated<LoggerEnrichmentConfiguration, LoggerConfiguration>, ILoggingEnrichmentConfiguration
	{
		public static LogContextEnricher Default { get; } = new LogContextEnricher();

		LogContextEnricher() : base(x => x.FromLogContext()) {}
	}
}
using Serilog;
using Serilog.Configuration;
using Super.Diagnostics.Logging.Configuration;
using Super.Model.Selection;

namespace Super.Diagnostics.Logging.Enrichment
{
	sealed class ExceptionHashEnricher : Select<LoggerEnrichmentConfiguration, LoggerConfiguration>,
	                                     ILoggingEnrichmentConfiguration
	{
		public static ExceptionHashEnricher Default { get; } = new ExceptionHashEnricher();

		ExceptionHashEnricher() : base(x => x.WithExceptionStackTraceHash()) {}
	}
}
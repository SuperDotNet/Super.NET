using Serilog;
using Serilog.Configuration;
using Super.Model.Selection;

namespace Super.Diagnostics {
	sealed class ExceptionHashEnricher : Select<LoggerEnrichmentConfiguration, LoggerConfiguration>, ILoggingEnrichmentConfiguration
	{
		public static ExceptionHashEnricher Default { get; } = new ExceptionHashEnricher();

		ExceptionHashEnricher() : base(x => x.WithExceptionStackTraceHash()) {}
	}
}
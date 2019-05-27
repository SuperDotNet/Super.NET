using Serilog;
using Serilog.Configuration;
using Super.Model.Selection;

namespace Super.Diagnostics {
	sealed class ProcessIdEnricher : Select<LoggerEnrichmentConfiguration, LoggerConfiguration>, ILoggingEnrichmentConfiguration
	{
		public static ProcessIdEnricher Default { get; } = new ProcessIdEnricher();

		ProcessIdEnricher() : base(x => x.WithProcessId()) {}
	}
}
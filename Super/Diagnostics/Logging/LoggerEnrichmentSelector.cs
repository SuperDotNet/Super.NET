using Serilog;
using Serilog.Configuration;
using Super.Model.Selection;

namespace Super.Diagnostics.Logging
{
	sealed class LoggerEnrichmentSelector : Delegated<LoggerConfiguration, LoggerEnrichmentConfiguration>
	{
		public static LoggerEnrichmentSelector Default { get; } = new LoggerEnrichmentSelector();

		LoggerEnrichmentSelector() : base(x => x.Enrich) {}
	}
}
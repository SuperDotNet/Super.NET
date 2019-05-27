using Serilog;
using Serilog.Configuration;
using Super.Model.Selection;

namespace Super.Diagnostics {
	public interface ILoggingEnrichmentConfiguration : ISelect<LoggerEnrichmentConfiguration, LoggerConfiguration> {}
}
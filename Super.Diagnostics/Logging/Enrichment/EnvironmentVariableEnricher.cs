using Serilog;
using Serilog.Configuration;
using Super.Diagnostics.Logging.Configuration;

namespace Super.Diagnostics.Logging.Enrichment
{
	sealed class EnvironmentVariableEnricher : ILoggingEnrichmentConfiguration
	{
		readonly string _name;

		public EnvironmentVariableEnricher(string name) => _name = name;

		public LoggerConfiguration Get(LoggerEnrichmentConfiguration parameter) => parameter.WithEnvironment(_name);
	}
}
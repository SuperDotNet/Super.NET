using Serilog;
using Serilog.Configuration;

namespace Super.Diagnostics {
	sealed class EnvironmentVariableEnricher : ILoggingEnrichmentConfiguration
	{
		readonly string _name;

		public EnvironmentVariableEnricher(string name) => _name = name;

		public LoggerConfiguration Get(LoggerEnrichmentConfiguration parameter) => parameter.WithEnvironment(_name);
	}
}
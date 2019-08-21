using Serilog;
using Serilog.Configuration;
using Super.Diagnostics.Logging.Configuration;
using Super.Model.Selection;

namespace Super.Diagnostics.Logging.Enrichment
{
	sealed class MachineNameEnricher : Select<LoggerEnrichmentConfiguration, LoggerConfiguration>, ILoggingEnrichmentConfiguration
	{
		public static MachineNameEnricher Default { get; } = new MachineNameEnricher();

		MachineNameEnricher() : base(ContextLoggerConfigurationExtension.WithMachineName) {}
	}
}
using Serilog;
using Serilog.Configuration;
using Super.Diagnostics.Logging.Configuration;
using Super.Model.Selection;

namespace Super.Diagnostics.Logging.Enrichment
{
	sealed class DomainUserNameEnricher : Select<LoggerEnrichmentConfiguration, LoggerConfiguration>,
	                                      ILoggingEnrichmentConfiguration
	{
		public static DomainUserNameEnricher Default { get; } = new DomainUserNameEnricher();

		DomainUserNameEnricher() : base(x => x.WithEnvironmentUserName()) {}
	}
}
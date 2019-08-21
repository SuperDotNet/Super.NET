using Serilog;
using Serilog.Configuration;
using Super.Diagnostics.Logging.Configuration;
using Super.Model.Selection;

namespace Super.Diagnostics.Logging.Enrichment
{
	sealed class UserNameEnricher : Select<LoggerEnrichmentConfiguration, LoggerConfiguration>,
	                                ILoggingEnrichmentConfiguration
	{
		public static UserNameEnricher Default { get; } = new UserNameEnricher();

		UserNameEnricher() : base(x => x.WithUserName()) {}
	}
}
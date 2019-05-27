using Serilog;
using Serilog.Configuration;
using Super.Model.Selection;

namespace Super.Diagnostics {
	sealed class DomainUserNameEnricher : Select<LoggerEnrichmentConfiguration, LoggerConfiguration>,
	                                      ILoggingEnrichmentConfiguration
	{
		public static DomainUserNameEnricher Default { get; } = new DomainUserNameEnricher();

		DomainUserNameEnricher() : base(x => x.WithEnvironmentUserName()) {}
	}
}
using Serilog;
using Serilog.Configuration;
using Super.Model.Selection;

namespace Super.Diagnostics {
	sealed class UserNameEnricher : Select<LoggerEnrichmentConfiguration, LoggerConfiguration>,
	                                ILoggingEnrichmentConfiguration
	{
		public static UserNameEnricher Default { get; } = new UserNameEnricher();

		UserNameEnricher() : base(x => x.WithUserName()) {}
	}
}
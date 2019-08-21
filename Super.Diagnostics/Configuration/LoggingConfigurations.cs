using Serilog;
using Super.Model.Selection.Alterations;

namespace Super.Diagnostics.Logging.Configuration
{
	class LoggingConfigurations : CompositeAlteration<LoggerConfiguration>, ILoggingConfiguration
	{
		public LoggingConfigurations(params IAlteration<LoggerConfiguration>[] alterations) : base(alterations) {}
	}
}
using Serilog;
using Super.Model.Sources.Alterations;

namespace Super.Diagnostics
{
	class LoggingConfigurations : CompositeAlteration<LoggerConfiguration>, ILoggingConfiguration
	{
		public LoggingConfigurations(params IAlteration<LoggerConfiguration>[] alterations) : base(alterations) {}
	}
}
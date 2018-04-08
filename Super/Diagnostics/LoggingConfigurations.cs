using Serilog;
using Super.Model.Selection.Alterations;

namespace Super.Diagnostics
{
	class LoggingConfigurations : CompositeAlteration<LoggerConfiguration>, ILoggingConfiguration
	{
		public LoggingConfigurations(params IAlteration<LoggerConfiguration>[] alterations) : base(alterations) {}
	}
}
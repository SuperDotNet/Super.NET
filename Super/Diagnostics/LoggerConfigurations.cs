using Serilog;
using Super.Model.Selection.Alterations;

namespace Super.Diagnostics
{
	public class LoggerConfigurations : CompositeAlteration<LoggerConfiguration>, ILoggingConfiguration
	{
		public LoggerConfigurations(params IAlteration<LoggerConfiguration>[] alterations) : base(alterations) {}
	}
}
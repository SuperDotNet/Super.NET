using Serilog;
using Super.Model.Selection.Alterations;

namespace Super.Diagnostics.Logging.Configuration
{
	public interface ILoggingConfiguration : IAlteration<LoggerConfiguration> {}
}
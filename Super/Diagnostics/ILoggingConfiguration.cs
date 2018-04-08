using Serilog;
using Super.Model.Selection.Alterations;

namespace Super.Diagnostics
{
	public interface ILoggingConfiguration : IAlteration<LoggerConfiguration> {}
}
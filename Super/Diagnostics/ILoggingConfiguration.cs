using Serilog;
using Super.Model.Sources.Alterations;

namespace Super.Diagnostics
{
	public interface ILoggingConfiguration : IAlteration<LoggerConfiguration> {}
}
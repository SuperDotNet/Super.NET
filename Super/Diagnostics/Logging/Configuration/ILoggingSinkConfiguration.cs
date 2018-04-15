using Serilog;
using Serilog.Configuration;
using Super.Model.Selection;

namespace Super.Diagnostics.Logging.Configuration
{
	public interface ILoggingSinkConfiguration : ISelect<LoggerSinkConfiguration, LoggerConfiguration> {}
}
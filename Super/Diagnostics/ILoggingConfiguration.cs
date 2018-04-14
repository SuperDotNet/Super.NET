using Serilog;
using Serilog.Configuration;
using Super.Model.Selection;
using Super.Model.Selection.Alterations;

namespace Super.Diagnostics
{
	public interface ILoggingConfiguration : IAlteration<LoggerConfiguration> {}

	public interface ILoggingSinkConfiguration : ISelect<LoggerSinkConfiguration, LoggerConfiguration> {}

	public interface ILoggingDestructureConfiguration : ISelect<LoggerDestructuringConfiguration, LoggerConfiguration> {}
}
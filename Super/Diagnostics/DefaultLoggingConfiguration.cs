using Serilog;
using Super.Model.Sources;

namespace Super.Diagnostics
{
	sealed class DefaultLoggingConfiguration : FixedParameterSource<LoggerConfiguration, LoggerConfiguration>
	{
		public static DefaultLoggingConfiguration Default { get; } = new DefaultLoggingConfiguration();

		DefaultLoggingConfiguration() : base(LoggingConfiguration.Default.Get(), new LoggerConfiguration()) {}
	}
}
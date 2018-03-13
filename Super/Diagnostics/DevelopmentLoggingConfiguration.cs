using Serilog;
using Super.Model.Instances;

namespace Super.Diagnostics
{
	sealed class DevelopmentLoggingConfiguration : IInstance<LoggerConfiguration>
	{
		public static DevelopmentLoggingConfiguration Default { get; } = new DevelopmentLoggingConfiguration();

		DevelopmentLoggingConfiguration() {}

		public LoggerConfiguration Get() => new LoggerConfiguration().MinimumLevel.Verbose()
		                                                             .WriteTo.ColoredConsole()
		                                                             .WriteTo.Trace();
	}
}
using Serilog;
using Super.Model.Instances;

namespace Super.Diagnostics
{
	sealed class LoggingConfiguration : IInstance<LoggerConfiguration>
	{
		public static LoggingConfiguration Default { get; } = new LoggingConfiguration();

		LoggingConfiguration() : this("http://localhost:5341") {}

		readonly string _serverUri;

		public LoggingConfiguration(string serverUri) => _serverUri = serverUri;

		public LoggerConfiguration Get()
			=> new LoggerConfiguration().MinimumLevel.Verbose()
			                            .WriteTo.ColoredConsole()
			                            .WriteTo.Trace()
			                            .WriteTo.Seq(_serverUri);
	}
}
using Super.Runtime.Environment;

namespace Super.Diagnostics.Logging.Configuration
{
	sealed class LoggingConfiguration : Component<ILoggingConfiguration>
	{
		public static LoggingConfiguration Default { get; } = new LoggingConfiguration();

		LoggingConfiguration() : base(DefaultLoggingConfiguration.Default) {}
	}
}
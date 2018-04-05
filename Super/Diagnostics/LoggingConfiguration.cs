using Super.Runtime.Environment;

namespace Super.Diagnostics
{
	sealed class LoggingConfiguration : Component<ILoggingConfiguration>
	{
		public static LoggingConfiguration Default { get; } = new LoggingConfiguration();

		LoggingConfiguration() : base(DefaultLoggingConfiguration.Default) {}
	}
}
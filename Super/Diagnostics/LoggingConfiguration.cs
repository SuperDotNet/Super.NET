using Super.Runtime.Environment;

namespace Super.Diagnostics
{
	sealed class LoggingConfiguration : Ambient<ILoggingConfiguration>
	{
		public static LoggingConfiguration Default { get; } = new LoggingConfiguration();

		LoggingConfiguration() : base(DefaultLoggingConfigurations.Default) {}
	}
}
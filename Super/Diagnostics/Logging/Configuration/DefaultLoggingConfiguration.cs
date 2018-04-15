using Super.ExtensionMethods;

namespace Super.Diagnostics.Logging.Configuration
{
	sealed class DefaultLoggingConfiguration : LoggingConfigurations
	{
		public static DefaultLoggingConfiguration Default { get; } = new DefaultLoggingConfiguration();

		DefaultLoggingConfiguration() : base(ControlledLoggingLevelConfiguration.Default,
		                                     TraceConfiguration.Default.ToConfiguration()) {}
	}
}
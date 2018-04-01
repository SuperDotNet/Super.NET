namespace Super.Diagnostics
{
	sealed class DefaultLoggingConfigurations : LoggingConfigurations
	{
		public static DefaultLoggingConfigurations Default { get; } = new DefaultLoggingConfigurations();

		DefaultLoggingConfigurations() : base(ControlledLoggingLevelConfiguration.Default, ConsoleConfiguration.Default,
		                                      TraceConfiguration.Default) {}
	}
}
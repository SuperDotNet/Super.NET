namespace Super.Diagnostics
{
	sealed class DefaultLoggingConfiguration : LoggingConfigurations
	{
		public static DefaultLoggingConfiguration Default { get; } = new DefaultLoggingConfiguration();

		DefaultLoggingConfiguration() : base(ControlledLoggingLevelConfiguration.Default,
		                                     TraceConfiguration.Default) {}
	}
}
using Serilog;
using Serilog.Core;

namespace Super.Diagnostics
{
	sealed class ControlledLoggingLevelConfiguration : ILoggingConfiguration
	{
		public static ControlledLoggingLevelConfiguration Default { get; } = new ControlledLoggingLevelConfiguration();

		ControlledLoggingLevelConfiguration() : this(LoggingLevelController.Default) {}

		readonly LoggingLevelSwitch _switch;

		public ControlledLoggingLevelConfiguration(LoggingLevelSwitch @switch) => _switch = @switch;

		public LoggerConfiguration Get(LoggerConfiguration parameter) => parameter.MinimumLevel.ControlledBy(_switch);
	}
}
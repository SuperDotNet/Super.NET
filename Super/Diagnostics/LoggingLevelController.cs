using Serilog.Core;
using Super.Model.Instances;

namespace Super.Diagnostics
{
	sealed class LoggingLevelController : Instance<LoggingLevelSwitch>
	{
		public static LoggingLevelController Default { get; } = new LoggingLevelController();

		LoggingLevelController() : this(new LoggingLevelSwitch(DefaultLoggingLevel.Default)) {}

		public LoggingLevelController(LoggingLevelSwitch instance) : base(instance) {}
	}
}
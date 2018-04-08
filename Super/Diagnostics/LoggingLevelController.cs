using Serilog.Core;
using Super.Model.Sources;

namespace Super.Diagnostics
{
	sealed class LoggingLevelController : Source<LoggingLevelSwitch>
	{
		public static LoggingLevelController Default { get; } = new LoggingLevelController();

		LoggingLevelController() : this(new LoggingLevelSwitch(DefaultLoggingLevel.Default)) {}

		public LoggingLevelController(LoggingLevelSwitch instance) : base(instance) {}
	}
}
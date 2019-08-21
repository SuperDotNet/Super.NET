using Serilog.Core;
using Super.Model.Results;

namespace Super.Diagnostics.Logging
{
	public sealed class LoggingLevelController : Instance<LoggingLevelSwitch>
	{
		public static LoggingLevelController Default { get; } = new LoggingLevelController();

		LoggingLevelController() : this(new LoggingLevelSwitch(DefaultLoggingLevel.Default)) {}

		public LoggingLevelController(LoggingLevelSwitch instance) : base(instance) {}
	}
}
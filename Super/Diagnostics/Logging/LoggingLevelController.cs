﻿using Serilog.Core;
using Super.Model.Selection.Sources;

namespace Super.Diagnostics.Logging
{
	public sealed class LoggingLevelController : Source<LoggingLevelSwitch>
	{
		public static LoggingLevelController Default { get; } = new LoggingLevelController();

		LoggingLevelController() : this(new LoggingLevelSwitch(DefaultLoggingLevel.Default)) {}

		public LoggingLevelController(LoggingLevelSwitch instance) : base(instance) {}
	}
}
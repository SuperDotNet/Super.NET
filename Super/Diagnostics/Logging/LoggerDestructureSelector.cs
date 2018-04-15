﻿using Serilog;
using Serilog.Configuration;
using Super.Model.Selection;

namespace Super.Diagnostics.Logging
{
	sealed class LoggerDestructureSelector : Delegated<LoggerConfiguration, LoggerDestructuringConfiguration>
	{
		public static LoggerDestructureSelector Default { get; } = new LoggerDestructureSelector();

		LoggerDestructureSelector() : base(x => x.Destructure) {}
	}
}
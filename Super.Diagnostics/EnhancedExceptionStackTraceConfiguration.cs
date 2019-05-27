﻿using Serilog;
using Serilog.Configuration;
using Super.Model.Selection;

namespace Super.Diagnostics {
	sealed class EnhancedExceptionStackTraceConfiguration : Select<LoggerEnrichmentConfiguration, LoggerConfiguration>,
	                                                        ILoggingEnrichmentConfiguration
	{
		public static EnhancedExceptionStackTraceConfiguration Default { get; } =
			new EnhancedExceptionStackTraceConfiguration();

		EnhancedExceptionStackTraceConfiguration() : base(x => x.WithDemystifiedStackTraces()) {}
	}
}
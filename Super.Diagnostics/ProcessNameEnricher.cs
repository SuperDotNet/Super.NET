﻿using Serilog;
using Serilog.Configuration;
using Super.Model.Selection;

namespace Super.Diagnostics {
	sealed class ProcessNameEnricher : Select<LoggerEnrichmentConfiguration, LoggerConfiguration>, ILoggingEnrichmentConfiguration
	{
		public static ProcessNameEnricher Default { get; } = new ProcessNameEnricher();

		ProcessNameEnricher() : base(x => x.WithProcessName()) {}
	}
}
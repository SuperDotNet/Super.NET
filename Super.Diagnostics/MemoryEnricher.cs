using Serilog;
using Serilog.Configuration;
using Super.Model.Selection;

namespace Super.Diagnostics {
	sealed class MemoryEnricher : Select<LoggerEnrichmentConfiguration, LoggerConfiguration>, ILoggingEnrichmentConfiguration
	{
		public static MemoryEnricher Default { get; } = new MemoryEnricher();

		MemoryEnricher() : base(x => x.WithMemoryUsage()) {}
	}
}
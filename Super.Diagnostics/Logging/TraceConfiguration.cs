using Serilog;
using Serilog.Configuration;
using Super.Diagnostics.Logging.Configuration;
using Super.Model.Selection;

namespace Super.Diagnostics.Logging
{
	sealed class TraceConfiguration : Select<LoggerSinkConfiguration, LoggerConfiguration>, ILoggingSinkConfiguration
	{
		public static TraceConfiguration Default { get; } = new TraceConfiguration();

		TraceConfiguration() : base(x => x.Trace()) {}
	}
}
using Serilog;
using Serilog.Configuration;
using Super.Model.Selection;

namespace Super.Diagnostics
{
	sealed class TraceConfiguration : Delegated<LoggerSinkConfiguration, LoggerConfiguration>, ILoggingSinkConfiguration
	{
		public static TraceConfiguration Default { get; } = new TraceConfiguration();

		TraceConfiguration() : base(x => x.Trace()) {}
	}
}
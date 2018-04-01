using Serilog;
using Super.Model.Sources.Alterations;

namespace Super.Diagnostics
{
	sealed class TraceConfiguration : DelegatedAlteration<LoggerConfiguration>, ILoggingConfiguration
	{
		public static TraceConfiguration Default { get; } = new TraceConfiguration();

		TraceConfiguration() : base(x => x.WriteTo.Trace()) {}
	}
}
using Serilog;
using Serilog.Configuration;
using Super.Model.Selection;

namespace Super.Diagnostics.Logging
{
	sealed class LoggerSinkSelector : Select<LoggerConfiguration, LoggerSinkConfiguration>
	{
		public static LoggerSinkSelector Default { get; } = new LoggerSinkSelector();

		LoggerSinkSelector() : base(x => x.WriteTo) {}
	}
}
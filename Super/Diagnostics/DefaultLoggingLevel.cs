using Serilog.Events;
using Super.Model.Sources;

namespace Super.Diagnostics
{
	sealed class DefaultLoggingLevel : Source<LogEventLevel>
	{
		public static DefaultLoggingLevel Default { get; } = new DefaultLoggingLevel();

		DefaultLoggingLevel() : base(LogEventLevel.Information) {}
	}
}
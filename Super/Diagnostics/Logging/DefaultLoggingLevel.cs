using Serilog.Events;
using Super.Model.Selection.Sources;

namespace Super.Diagnostics.Logging
{
	sealed class DefaultLoggingLevel : Source<LogEventLevel>
	{
		public static DefaultLoggingLevel Default { get; } = new DefaultLoggingLevel();

		DefaultLoggingLevel() : base(LogEventLevel.Information) {}
	}
}
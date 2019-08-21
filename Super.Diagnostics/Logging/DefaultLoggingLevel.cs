using Serilog.Events;
using Super.Model.Results;

namespace Super.Diagnostics.Logging
{
	sealed class DefaultLoggingLevel : Instance<LogEventLevel>
	{
		public static DefaultLoggingLevel Default { get; } = new DefaultLoggingLevel();

		DefaultLoggingLevel() : base(LogEventLevel.Information) {}
	}
}
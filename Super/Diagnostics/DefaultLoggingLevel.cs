using Serilog.Events;
using Super.Model.Instances;

namespace Super.Diagnostics
{
	sealed class DefaultLoggingLevel : Instance<LogEventLevel>
	{
		public static DefaultLoggingLevel Default { get; } = new DefaultLoggingLevel();

		DefaultLoggingLevel() : base(LogEventLevel.Information) {}
	}
}
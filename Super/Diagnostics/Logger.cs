using Serilog;
using Super.ExtensionMethods;
using Super.Model.Instances;

namespace Super.Diagnostics
{
	public sealed class Logger : DeferredInstance<ILogger>
	{
		public static Logger Default { get; } = new Logger();

		Logger() : this(LoggingConfiguration.Default.Get()) {}

		public Logger(LoggerConfiguration configuration) : base(Loggers.Default.Fix(configuration)) {}
	}
}
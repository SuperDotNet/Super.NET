using Serilog;
using Super.ExtensionMethods;
using Super.Model.Instances;

namespace Super.Diagnostics
{
	public sealed class Logger : DeferredInstance<ILogger>
	{
		public static Logger Default { get; } = new Logger();

		Logger() : base(DefaultLoggingConfiguration.Default.Adapt(LoggerCoercer.Default)) {}
	}
}
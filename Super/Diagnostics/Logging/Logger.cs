using Serilog;
using Super.Diagnostics.Logging.Configuration;
using Super.ExtensionMethods;
using Super.Model.Selection;
using Super.Runtime.Activation;
using Super.Runtime.Execution;

namespace Super.Diagnostics.Logging
{
	public sealed class Logger : Ambient<IPrimaryLogger>
	{
		public static Logger Default { get; } = new Logger();

		Logger() : base(LoggingConfiguration.Default
		                                    .Select(Activation<LoggerConfiguration>.Default.Get)
		                                    .Select(LoggerSelector.Default)
		                                    .Select(New<PrimaryLogger>.Default)
		                                    .Get) {}
	}
}
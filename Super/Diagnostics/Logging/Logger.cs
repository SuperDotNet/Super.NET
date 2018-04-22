using Serilog;
using Super.Diagnostics.Logging.Configuration;
using Super.Model.Sources;
using Super.Runtime.Activation;

namespace Super.Diagnostics.Logging
{
	public sealed class Logger : DecoratedSource<IPrimaryLogger>
	{
		public static Logger Default { get; } = new Logger();

		Logger() : base(LoggingConfiguration.Default
		                                    .Select(Activation<LoggerConfiguration>.Default.Get)
		                                    .Select(LoggerSelector.Default)
		                                    .Select(Model.Selection.New<PrimaryLogger>.Default)) {}
	}
}
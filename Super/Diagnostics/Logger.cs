using Serilog;
using Super.ExtensionMethods;
using Super.Model.Selection;
using Super.Runtime;
using Super.Runtime.Activation;

namespace Super.Diagnostics
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
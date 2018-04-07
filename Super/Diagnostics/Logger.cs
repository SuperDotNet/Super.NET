using Serilog;
using Super.ExtensionMethods;
using Super.Runtime;
using Super.Runtime.Activation;

namespace Super.Diagnostics
{
	public sealed class Logger : Ambient<IPrimaryLogger>
	{
		public static Logger Default { get; } = new Logger();

		Logger() : base(LoggingConfiguration.Default
		                                    .Select(Activation<LoggerConfiguration>.Default.Get)
		                                    .Select(LoggerCoercer.Default)
		                                    .Select(Model.Containers.New<PrimaryLogger>.Default)
		                                    .Get) {}
	}
}
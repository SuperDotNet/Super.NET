using Super.ExtensionMethods;
using Super.Reflection;
using Super.Runtime;

namespace Super.Diagnostics
{
	public sealed class Logger : Ambient<IPrimaryLogger>
	{
		public static Logger Default { get; } = new Logger();

		Logger() : base(LoggingConfiguration.Default
		                                    .Adapt()
		                                    .Reduce()
		                                    .Out(LoggerCoercer.Default)
		                                    .Out(I<PrimaryLogger>.Default)
		                                    .ToInstance()) {}
	}
}
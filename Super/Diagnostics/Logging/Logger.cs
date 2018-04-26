using Super.Diagnostics.Logging.Configuration;
using Super.Model.Sources;
using Super.Reflection;

namespace Super.Diagnostics.Logging
{
	public sealed class Logger : DecoratedSource<IPrimaryLogger>
	{
		public static Logger Default { get; } = new Logger();

		Logger() : base(LoggingConfiguration.Default
		                                    .Select(x => x.Fold()
		                                                  .Out(LoggerSelector.Default)
		                                                  .Activate(I<PrimaryLogger>.Default))) {}
	}
}
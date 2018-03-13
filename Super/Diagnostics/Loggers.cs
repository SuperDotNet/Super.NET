using Serilog;
using Super.Model.Sources;

namespace Super.Diagnostics
{
	public sealed class Loggers : ISource<LoggerConfiguration, ILogger>
	{
		public static Loggers Default { get; } = new Loggers();

		Loggers() {}

		public ILogger Get(LoggerConfiguration parameter) => parameter.CreateLogger();
	}
}
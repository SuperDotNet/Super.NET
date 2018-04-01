using Serilog;
using Super.Model.Sources;

namespace Super.Diagnostics
{
	public sealed class LoggerCoercer : ISource<LoggerConfiguration, ILogger>
	{
		public static LoggerCoercer Default { get; } = new LoggerCoercer();

		LoggerCoercer() {}

		public ILogger Get(LoggerConfiguration parameter) => parameter.CreateLogger();
	}
}
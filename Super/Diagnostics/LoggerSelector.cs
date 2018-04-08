using Serilog;
using Super.Model.Selection;

namespace Super.Diagnostics
{
	public sealed class LoggerSelector : ISelect<LoggerConfiguration, ILogger>
	{
		public static LoggerSelector Default { get; } = new LoggerSelector();

		LoggerSelector() {}

		public ILogger Get(LoggerConfiguration parameter) => parameter.CreateLogger();
	}
}
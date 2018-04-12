using Serilog;
using Super.Model.Selection;

namespace Super.Diagnostics
{
	public sealed class LoggerSelector : Delegated<LoggerConfiguration, ILogger>
	{
		public static LoggerSelector Default { get; } = new LoggerSelector();

		LoggerSelector() : base(x => x.CreateLogger()) {}
	}
}
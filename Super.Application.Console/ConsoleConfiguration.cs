using Serilog;
using Super.Diagnostics;
using Super.Model.Selection.Alterations;

namespace Super.Application.Console
{
	sealed class ConsoleConfiguration : DelegatedAlteration<LoggerConfiguration>, ILoggingConfiguration
	{
		public static ConsoleConfiguration Default { get; } = new ConsoleConfiguration();

		ConsoleConfiguration() : base(x => x.WriteTo.ColoredConsole()) {}
	}
}
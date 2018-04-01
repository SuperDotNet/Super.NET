using Serilog;
using Super.Model.Sources.Alterations;

namespace Super.Diagnostics
{
	sealed class ConsoleConfiguration : DelegatedAlteration<LoggerConfiguration>, ILoggingConfiguration
	{
		public static ConsoleConfiguration Default { get; } = new ConsoleConfiguration();

		ConsoleConfiguration() : base(x => x.WriteTo.ColoredConsole()) {}
	}
}
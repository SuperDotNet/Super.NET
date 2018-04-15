using Serilog;
using Serilog.Configuration;
using Super.Diagnostics.Logging;
using Super.Diagnostics.Logging.Configuration;
using Super.Model.Sources;
using System;

namespace Super.Application.Console
{
	public sealed class ConsoleConfiguration : ILoggingSinkConfiguration
	{
		readonly IFormatProvider _provider;
		readonly LoggingLevelController _controller;
		readonly string _template;

		public ConsoleConfiguration(IFormatProvider provider)
			: this(provider, LoggingLevelController.Default, ConsoleMessageTemplate.Default) {}

		public ConsoleConfiguration(IFormatProvider provider, LoggingLevelController controller, string template)
		{
			_provider = provider;
			_controller = controller;
			_template = template;
		}

		public LoggerConfiguration Get(LoggerSinkConfiguration parameter) => parameter.Console(formatProvider: _provider, outputTemplate: _template, levelSwitch: _controller);
	}

	sealed class ConsoleMessageTemplate : Source<string>
	{
		public static ConsoleMessageTemplate Default { get; } = new ConsoleMessageTemplate();

		ConsoleMessageTemplate() : base("[{Timestamp:HH:mm:ss} {Level:u3}] {Message:l}{NewLine}{Exception}") {}
	}
}
using System;
using Serilog;
using Serilog.Configuration;
using Super.Diagnostics.Logging;
using Super.Diagnostics.Logging.Configuration;

namespace Super.Application.Host.Console
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

	sealed class ConsoleMessageTemplate : Text.Text
	{
		public static ConsoleMessageTemplate Default { get; } = new ConsoleMessageTemplate();

		ConsoleMessageTemplate() : base($"{TemplateHeader.Default} {{Message:l}}{{NewLine}}{{Exception}}") {}
	}
}
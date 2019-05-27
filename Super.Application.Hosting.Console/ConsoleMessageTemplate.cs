using Super.Diagnostics.Logging;

namespace Super.Application.Hosting.Console
{
	sealed class ConsoleMessageTemplate : Text.Text
	{
		public static ConsoleMessageTemplate Default { get; } = new ConsoleMessageTemplate();

		ConsoleMessageTemplate() : base($"{TemplateHeader.Default} {{Message:l}}{{NewLine}}{{Exception}}") {}
	}
}
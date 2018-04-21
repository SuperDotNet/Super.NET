using Super.Text.Formatting;

namespace Super.Diagnostics.Logging
{
	public sealed class OutputTemplate : Text.Text
	{
		public static OutputTemplate Default { get; } = new OutputTemplate();

		OutputTemplate() : base($"{TemplateHeader.Default} ({{SourceContext}}) {{Message}}{{NewLine}}{{Exception}}") {}
	}

	public sealed class TemplateHeader : Text.Text
	{
		public static TemplateHeader Default { get; } = new TemplateHeader();

		TemplateHeader() : base($"[{{Timestamp:{TimestampFormat.Default}}} {{Level:u3}}]") {}
	}

	public sealed class TimestampFormat : Text.Text
	{
		public static TimestampFormat Default { get; } = new TimestampFormat();

		TimestampFormat() : base("HH:mm:ss:fff") {}
	}

	public sealed class TimestampFormatter : DateTimeOffsetFormatter
	{
		public static TimestampFormatter Default { get; } = new TimestampFormatter();

		TimestampFormatter() : base(TimestampFormat.Default) {}
	}
}
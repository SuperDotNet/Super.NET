namespace Super.Diagnostics.Logging.Text
{
	public sealed class TemplateHeader : Super.Text.Text
	{
		public static TemplateHeader Default { get; } = new TemplateHeader();

		TemplateHeader() : base($"[{{Timestamp:{TimestampFormat.Default}}} {{Level:u3}}]") {}
	}
}
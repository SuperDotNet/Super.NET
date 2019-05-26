namespace Super.Diagnostics.Logging
{
	public sealed class TemplateHeader : Text.Text
	{
		public static TemplateHeader Default { get; } = new TemplateHeader();

		TemplateHeader() : base($"[{{Timestamp:{TimestampFormat.Default}}} {{Level:u3}}]") {}
	}
}
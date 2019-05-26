namespace Super.Diagnostics.Logging
{
	public sealed class OutputTemplate : Text.Text
	{
		public static OutputTemplate Default { get; } = new OutputTemplate();

		OutputTemplate() : base($"{TemplateHeader.Default} ({{SourceContext}}) {{Message}}{{NewLine}}{{Exception}}") {}
	}
}
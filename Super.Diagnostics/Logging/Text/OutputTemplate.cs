﻿namespace Super.Diagnostics.Logging.Text
{
	public sealed class OutputTemplate : Super.Text.Text
	{
		public static OutputTemplate Default { get; } = new OutputTemplate();

		OutputTemplate() : base($"{TemplateHeader.Default} ({{SourceContext}}) {{Message}}{{NewLine}}{{Exception}}") {}
	}
}
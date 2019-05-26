using System;
using Super.Model.Selection.Conditions;
using Super.Runtime.Environment;

namespace Super.Text.Formatting
{
	public sealed class FormatterRegistration : SystemStore<IConditional<object, IFormattable>>
	{
		public static FormatterRegistration Default { get; } = new FormatterRegistration();

		FormatterRegistration() : base(DefaultSystemFormatter.Default.Self) {}
	}
}
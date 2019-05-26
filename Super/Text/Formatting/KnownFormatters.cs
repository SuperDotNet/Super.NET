using System;
using Super.Model.Selection;

namespace Super.Text.Formatting
{
	sealed class KnownFormatters : Select<object, IFormattable>
	{
		public static KnownFormatters Default { get; } = new KnownFormatters();

		KnownFormatters() : base(FormatterRegistration.Default.Get) {}
	}
}
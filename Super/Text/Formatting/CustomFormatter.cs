using System;
using Super.Model.Selection;

namespace Super.Text.Formatting
{
	sealed class CustomFormatter : ICustomFormatter
	{
		public static CustomFormatter Default { get; } = new CustomFormatter();

		CustomFormatter() : this(Formatters.Default) {}

		readonly ISelect<object, IFormattable> _select;

		public CustomFormatter(ISelect<object, IFormattable> table) => _select = table;

		public string Format(string format, object arg, IFormatProvider formatProvider)
			=> _select.Get(arg).ToString(format, formatProvider);
	}
}
using Super.Runtime.Activation;
using System;
using System.Globalization;

namespace Super.Text.Formatting
{
	sealed class FormatProvider : ServiceProvider, IFormatProvider
	{
		public static FormatProvider Default { get; } = new FormatProvider();

		FormatProvider() : base(CustomFormatter.Default, CultureInfo.CurrentCulture) {}

		public object GetFormat(Type formatType) => GetService(formatType);
	}
}
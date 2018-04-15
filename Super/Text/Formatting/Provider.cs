using System;
using System.Globalization;
using Super.Runtime.Activation;

namespace Super.Text.Formatting
{
	sealed class Provider : ServiceProvider, IFormatProvider
	{
		public static Provider Default { get; } = new Provider();

		Provider() : base(CustomFormatter.Default, CultureInfo.CurrentCulture) {}

		public object GetFormat(Type formatType) => GetService(formatType);
	}
}
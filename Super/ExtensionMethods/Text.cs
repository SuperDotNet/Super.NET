using Super.Model.Selection;
using Super.Reflection;
using Super.Runtime.Activation;
using Super.Text.Formatting;
using System;

namespace Super.ExtensionMethods
{
	public static class Text
	{
		public static ISelect<object, Func<object, IFormattable>> Register<T>(this INamedFormatter<T> @this)
			=> @this.To(I<Formatters<T>>.Default).In(Cast<object>.Default).ToDelegate().OrDefault(IsTypeSpecification<T>.Default);

		public static ISelect<string, T, string> AsDefault<T>(this ISelect<T, string> @this)
			=> @this.ToDelegate().Allow(I<string>.Default);
	}
}

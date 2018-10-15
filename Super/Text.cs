using Super.Model.Selection;
using Super.Reflection;
using Super.Reflection.Types;
using Super.Text;
using Super.Text.Formatting;
using System;

namespace Super
{
	// ReSharper disable once MismatchedFileName

	public static partial class ExtensionMethods
	{
		public static ISpecification<object, IFormattable> Register<T>(this ISelectFormatter<T> @this)
			=> Super.Start.From<object>()
			        .Cast<T>()
			        .Select(@this.To(I<Formatters<T>>.Default))
			        .To(IsType<T>.Default.Out);

		public static string OrNone<T>(this T @this) => @this?.ToString() ?? None.Default;

		public static string OrNone<T>(this T? @this) where T : struct => @this?.ToString() ?? None.Default;
	}
}
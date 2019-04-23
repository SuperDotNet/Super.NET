using Super.Model.Selection.Conditions;
using Super.Reflection.Types;
using Super.Text;
using Super.Text.Formatting;
using System;

namespace Super
{
	// ReSharper disable once MismatchedFileName

	public static partial class ExtensionMethods
	{
		public static IConditional<object, IFormattable> Register<T>(this ISelectFormatter<T> @this)
			=> Compose.Start.A.Selection.Of.Any.AndOf<T>()
			          .By.Cast.Or.Throw.Select(new Formatters<T>(@this))
			          .ToConditional(IsOf<T>.Default);

		public static string OrNone<T>(this T @this) => @this?.ToString() ?? None.Default;

		public static string OrNone<T>(this T? @this) where T : struct => @this?.ToString() ?? None.Default;
	}
}
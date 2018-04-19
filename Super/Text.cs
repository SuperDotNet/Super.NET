using Super.Model.Selection;
using Super.Reflection;
using Super.Reflection.Types;
using Super.Runtime.Activation;
using Super.Text;
using Super.Text.Formatting;
using System;

namespace Super
{
	public static partial class ExtensionMethods
	{
		public static ISelect<object, Func<object, IFormattable>> Register<T>(this ISelectFormatter<T> @this)
			=> @this.To(I<Formatters<T>>.Default)
			        .In(Cast<object>.Default)
			        .ToDelegate()
			        .OrDefault(IsType<T>.Default);

		public static ISelect<string, TParameter, TResult> AsDefault<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this)
			=> @this.ToDelegate().Allow(I<string>.Default);

		public static string OrNone<T>(this T @this) => @this?.ToString() ?? None.Default;

		public static string OrNone<T>(this T? @this) where T : struct => @this?.ToString() ?? None.Default;
	}
}
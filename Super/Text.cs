using Super.Model.Selection;
using Super.Reflection;
using Super.Reflection.Types;
using Super.Text;
using Super.Text.Formatting;
using System;

namespace Super
{
	public static partial class ExtensionMethods
	{
		public static ISelect<object, Func<object, IFormattable>> Register<T>(this ISelectFormatter<T> @this)
			=> IsType<T>.Default.If(In<object>.Cast<T>()
			                                  .Select(@this.To(I<Formatters<T>>.Default))
			                                  .ToDelegate()
			                                  .Allow(I<object>.Default));

		public static ISelect<string, TParameter, TResult> AsDefault<TParameter, TResult>(
			this ISelect<TParameter, TResult> @this) => @this.ToDelegate().Allow(I<string>.Default);

		public static string OrNone<T>(this T @this) => @this?.ToString() ?? None.Default;

		public static string OrNone<T>(this T? @this) where T : struct => @this?.ToString() ?? None.Default;

		public static ISelect<TParameter, TIn, TOut> Allow<TParameter, TIn, TOut>(this Func<TIn, TOut> @this,
		                                                                          I<TParameter> infer)
			=> I<Select<TParameter, TIn, TOut>>.Default.From(@this.ToSource().Out(infer).ToDelegate());
	}
}
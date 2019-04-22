using Super.Model.Selection.Conditions;
using Super.Reflection;

namespace Super
{
	// ReSharper disable once MismatchedFileName

	public static partial class ExtensionMethods
	{
		public static ICondition<T> Equal<T>(this T @this) => I.A<EqualityCondition<T>>().From(@this);

		public static ICondition<T> Not<T>(this T @this) => @this.Equal().Then().Inverse().Get().ToCondition();
	}
}
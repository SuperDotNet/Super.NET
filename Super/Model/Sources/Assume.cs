using Super.ExtensionMethods;
using Super.Reflection;

namespace Super.Model.Sources
{
	public static class Assume<T>
	{
		public static ISource<T, TResult> Default<TResult>(TResult @this) => I<Fixed<T, TResult>>.Default.Get(@this);
	}
}
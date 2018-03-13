using Super.ExtensionMethods;
using Super.Reflection;

namespace Super.Model.Sources
{
	public static class Assume<T>
	{
		public static ISource<T, TResult> Default<TResult>(TResult @this) => I<Instance<T, TResult>>.Default.Get(@this);
	}
}
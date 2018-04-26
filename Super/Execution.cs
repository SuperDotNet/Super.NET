using Super.Model.Sources;
using Super.Runtime.Execution;

namespace Super
{
	public static class Execution
	{
		public static ISource<T> ToAmbient<T>(this ISource<T> @this) => @this.To(AmbientAlteration<T>.Default);
	}
}

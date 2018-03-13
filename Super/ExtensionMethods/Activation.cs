using Super.Model.Sources;
using Super.Reflection;
using Super.Runtime.Activation;

namespace Super.ExtensionMethods
{
	static class Activation
	{
		public static T Get<T, TParameter>(this I<T> _, TParameter parameter) where T : class, IActivateMarker<TParameter>
			=> Instances<TParameter, T>.Default.Get(parameter);

		public static ISource<TParameter, T> Infer<T, TParameter>(this I<T> _, TParameter __ = default)
			where T : class, IActivateMarker<TParameter>
			=> Instances<TParameter, T>.Default;
	}
}
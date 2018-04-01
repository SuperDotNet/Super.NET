using Super.Reflection;

namespace Super.Runtime.Activation
{
	public static class Locator<T>
	{
		public static T New<TParameter>(TParameter parameter) => New<TParameter, T>.Default.Get(parameter);

		public static T New() => Activation.New<T>.Default.Get();

		public static T Get() => Activator<T>.Default.Get();
	}

	public static class Locator
	{
		public static T New<TParameter, T>(this I<T> _, TParameter parameter) where T : IActivateMarker<TParameter>
			=> Locator<T>.New(parameter);
	}
}
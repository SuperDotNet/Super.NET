using Super.Reflection;

namespace Super.Runtime.Activation
{
	public static class Activate<T>
	{
		public static T New<TParameter>(TParameter parameter) => Activation<TParameter, T>.Default.Get(parameter);

		public static T New() => Activation<T>.Default.Get();

		public static T Get() => Activator<T>.Default.Get();
	}

	public static class Activate
	{
		public static T From<TParameter, T>(this I<T> _, TParameter parameter) where T : IActivateMarker<TParameter>
			=> Activations<TParameter, T>.Default.Get(parameter);
	}

	public static class From
	{
		public static TTo New<TFrom, TTo>(this TFrom @this, I<TTo> _) => Activate<TTo>.New(@this);

		public static TTo To<TFrom, TTo>(this TFrom @this, I<TTo> select) where TTo : IActivateMarker<TFrom>
			=> select.From(@this);
	}
}
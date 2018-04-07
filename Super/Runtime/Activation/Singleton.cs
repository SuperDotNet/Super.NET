using Super.ExtensionMethods;
using Super.Model.Containers;
using Super.Model.Instances;
using Super.Reflection;

namespace Super.Runtime.Activation
{
	sealed class Singleton<T> : DeferredSingleton<T>, ISingleton<T>
	{
		public static Singleton<T> Default { get; } = new Singleton<T>();

		Singleton() : base(Singletons.Default.Select(Types<T>.Identity).Select(Cast<T>.Default).Get) {}
	}
}
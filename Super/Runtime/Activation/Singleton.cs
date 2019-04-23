using Super.Model.Results;
using Super.Reflection.Types;

namespace Super.Runtime.Activation
{
	sealed class Singleton<T> : DeferredSingleton<T>, ISingleton<T>
	{
		public static Singleton<T> Default { get; } = new Singleton<T>();

		Singleton() : base(Singletons.Default.In(Type<T>.Instance).Then().Cast<T>().Selector()) {}
	}
}
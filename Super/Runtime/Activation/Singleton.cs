using Super.Model.Sources;
using Super.Reflection;
using Super.Reflection.Types;

namespace Super.Runtime.Activation
{
	sealed class Singleton<T> : DeferredSingleton<T>, ISingleton<T>
	{
		public static Singleton<T> Default { get; } = new Singleton<T>();

		Singleton() : base(Singletons.Default
		                             .Select(Type<T>.Instance)
		                             .Cast(I<T>.Default)
		                             .Out()) {}
	}
}
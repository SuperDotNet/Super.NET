using Super.Compose;

namespace Super.Runtime.Environment
{
	sealed class DefaultComponentLocator<T> : SystemStore<T>
	{
		public static implicit operator T(DefaultComponentLocator<T> instance) => instance.Get();

		public static DefaultComponentLocator<T> Default { get; } = new DefaultComponentLocator<T>();

		DefaultComponentLocator() : base(Start.A.Selection<T>()
		                                      .By.Self.If(LocateGuard<T>.Default)
		                                      .In(ComponentLocator<T>.Default)) {}
	}
}
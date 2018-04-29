namespace Super.Runtime.Environment
{
	sealed class DefaultComponent<T> : SystemStore<T>
	{
		public static DefaultComponent<T> Default { get; } = new DefaultComponent<T>();

		DefaultComponent() : base(ComponentLocator<T>.Default.Unless(LocateGuard<T>.Default)) {}
	}
}
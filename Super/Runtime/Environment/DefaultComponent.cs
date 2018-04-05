namespace Super.Runtime.Environment
{
	sealed class DefaultComponent<T> : Component<T>
	{
		public static DefaultComponent<T> Default { get; } = new DefaultComponent<T>();

		DefaultComponent() {}
	}
}
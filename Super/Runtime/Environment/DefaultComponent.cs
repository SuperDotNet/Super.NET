namespace Super.Runtime.Environment
{
	sealed class RequiredComponent<T> : SystemAssignment<T>
	{
		public static RequiredComponent<T> Default { get; } = new RequiredComponent<T>();

		RequiredComponent() : base(ComponentLocator<T>.Default.Unless(LocateGuard<T>.Default)) {}
	}
}
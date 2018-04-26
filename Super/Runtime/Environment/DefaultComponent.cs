namespace Super.Runtime.Environment
{
	/*sealed class DefaultComponent<T> : ComponentAssignment<T>
	{
		public static DefaultComponent<T> Default { get; } = new DefaultComponent<T>();

		DefaultComponent() {}
	}*/

	sealed class RequiredComponent<T> : SystemAssignment<T>
	{
		public static RequiredComponent<T> Default { get; } = new RequiredComponent<T>();

		RequiredComponent() : base(ComponentLocator<T>.Default
		                                              .Select(x => x.Or(In<T>.Type(LocateMessage.Default)))
		                                              .ToDelegate()) {}
	}
}
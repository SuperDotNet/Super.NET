namespace Super.Runtime.Environment
{
	sealed class Locate<T> : Ambient<T>
	{
		public static Locate<T> Default { get; } = new Locate<T>();

		Locate() : base(Locator<T>.Default) {}
	}
}
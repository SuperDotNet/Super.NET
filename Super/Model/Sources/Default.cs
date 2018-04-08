namespace Super.Model.Sources
{
	sealed class Default<T> : Source<T>
	{
		public static Default<T> Instance { get; } = new Default<T>();

		Default() : base(default) {}
	}
}
namespace Super.Model.Sources
{
	sealed class Default<T> : Fixed<T, T>
	{
		public static Default<T> Instance { get; } = new Default<T>();

		Default() : base(default) {}
	}

	sealed class Default<TParameter, TResult> : Fixed<TParameter, TResult>
	{
		public static Default<TParameter, TResult> Instance { get; } = new Default<TParameter, TResult>();

		Default() : base(default) {}
	}
}
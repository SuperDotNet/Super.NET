using Super.Model.Sources;

namespace Super.Runtime
{
	sealed class Default<T> : Instance<T, T>
	{
		public static Default<T> Instance { get; } = new Default<T>();

		Default() : base(default) {}
	}

	sealed class Default<TParameter, TResult> : Instance<TParameter, TResult>
	{
		public static Default<TParameter, TResult> Instance { get; } = new Default<TParameter, TResult>();

		Default() : base(default) {}
	}
}
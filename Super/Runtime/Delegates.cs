using System;

namespace Super.Runtime
{
	public static class Delegates<T>
	{
		public static Action<T> Empty { get; } = t => {};

		public static Func<T, T> Self { get; } = t => t;
	}

	public static class Delegates
	{
		public static Action Empty { get; } = () => {};
	}
}
﻿namespace Super.Model.Selection
{
	sealed class Default<T> : FixedResult<T, T>
	{
		public static Default<T> Instance { get; } = new Default<T>();

		Default() : base(default) {}
	}

	sealed class Default<TParameter, TResult> : FixedResult<TParameter, TResult>
	{
		public static ISelect<TParameter, TResult> Instance { get; } = new Default<TParameter, TResult>();

		Default() : base(default) {}
	}
}
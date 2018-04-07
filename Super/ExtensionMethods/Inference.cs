using Super.Model.Sources;
using Super.Reflection;
using System;

namespace Super.ExtensionMethods
{
	public static class Inference
	{
		/*public static (I<T>, TOther) Pair<T, TOther>(this I<T> @this, Func<T, TOther> other) => @this.Pair(@this.From(other));

		public static TOther From<T, TOther>(this I<T> _, Func<T, TOther> other) => other(default);*/

		public static I<T, TParameter, TResult> Source<T, TParameter, TResult>(this I<T> _,
		                                                                       Func<T, ISource<TParameter, TResult>> __)
			where T : ISource<TParameter, TResult> => I<T, TParameter, TResult>.Default;

		public static ISource<TParameter, TResult> Try<TException, TParameter, TResult>(
			this I<TException> @this, Func<TParameter, TResult> source, Func<TParameter, TResult> fallback)
			where TException : Exception
			=> new Try<TException, TParameter, TResult>(source, fallback);
	}
}
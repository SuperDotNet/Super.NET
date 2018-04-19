using Super.Model.Selection;
using Super.Reflection;
using System;

namespace Super
{
	public static partial class ExtensionMethods
	{
		public static I<T, TParameter, TResult> Source<T, TParameter, TResult>(this I<T> _,
		                                                                       Func<T, ISelect<TParameter, TResult>> __)
			where T : ISelect<TParameter, TResult> => I<T, TParameter, TResult>.Default;

		public static ISelect<TParameter, TResult> Try<TException, TParameter, TResult>(
			this I<TException> @this, Func<TParameter, TResult> source, Func<TParameter, TResult> fallback)
			where TException : Exception
			=> new Try<TException, TParameter, TResult>(source, fallback);
	}
}
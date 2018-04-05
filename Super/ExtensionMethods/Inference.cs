using Super.Model.Sources;
using Super.Reflection;
using System;

namespace Super.ExtensionMethods
{
	public static class Inference
	{
		public static (I<T>, TOther) Pair<T, TOther>(this I<T> @this, Func<T, TOther> other) => @this.Pair(@this.From(other));

		public static TOther From<T, TOther>(this I<T> _, Func<T, TOther> other) => other(default);

		public static Source<T, TParameter, TResult> Source<T, TParameter, TResult>(this I<T> _,
		                                                                            Func<T, ISource<TParameter, TResult>> __)
			where T : ISource<TParameter, TResult> => Super.Reflection.Source<T, TParameter, TResult>.Default;
	}
}
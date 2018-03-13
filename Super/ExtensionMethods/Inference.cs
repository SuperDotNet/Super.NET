using System;
using Super.Reflection;

namespace Super.ExtensionMethods
{
	public static class Inference
	{
		public static (I<T>, TOther) Pair<T, TOther>(this I<T> @this, Func<T, TOther> other) => @this.Pair(@this.From(other));

		public static TOther From<T, TOther>(this I<T> _, Func<T, TOther> other) => other(default);
	}
}
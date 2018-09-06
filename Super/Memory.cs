using System;
using System.Collections.Generic;

namespace Super
{
	// ReSharper disable once MismatchedFileName
	public static partial class ExtensionMethods
	{
		public static IEnumerable<T> AsEnumerable<T>(this ReadOnlyMemory<T> @this) => @this.Span.ToArray();

		public static T[] Get<T>(this ReadOnlyMemory<T> @this) => @this.Span.ToArray();

		public static bool Any<T>(this ReadOnlyMemory<T> @this) => !@this.IsEmpty;
	}
}

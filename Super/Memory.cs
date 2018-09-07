using System;
using System.Collections.Generic;
using System.Linq;
using Super.Model.Collections;

namespace Super
{
	// ReSharper disable once MismatchedFileName
	public static partial class ExtensionMethods
	{
		public static IEnumerable<T> AsEnumerable<T>(this IArray<T> @this) => @this.Get().AsEnumerable();

		public static IEnumerable<T> AsEnumerable<T>(this ReadOnlyMemory<T> @this) => @this.Span.ToArray().Hide();

		public static T[] Get<T>(this ReadOnlyMemory<T> @this) => @this.Span.ToArray();

		public static bool Any<T>(this ReadOnlyMemory<T> @this) => !@this.IsEmpty;
	}
}

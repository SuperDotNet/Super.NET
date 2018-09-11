using Super.Model.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Super
{
	// ReSharper disable once MismatchedFileName
	public static partial class ExtensionMethods
	{
		public static IEnumerable<T> AsEnumerable<T>(this IArray<T> @this) => @this.Get().AsEnumerable();

		public static IEnumerable<T> AsEnumerable<T>(this ReadOnlyMemory<T> @this) => MemoryMarshal.ToEnumerable(@this);

		public static ArraySegment<T>? Source<T>(this ReadOnlyMemory<T> @this)
			=> MemoryMarshal.TryGetArray(@this, out var result) ? result : (ArraySegment<T>?)null;

		public static bool Any<T>(this ReadOnlyMemory<T> @this) => !@this.IsEmpty;
	}
}

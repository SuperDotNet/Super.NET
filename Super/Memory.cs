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

		public static ArraySegment<T>? Segment<T>(this ReadOnlyMemory<T> @this)
			=> MemoryMarshal.TryGetArray(@this, out var result) ? result : (ArraySegment<T>?)null;

		public static T[] Get<T>(this ReadOnlyMemory<T> @this) => @this.Segment()?.Array ?? @this.ToArray();

		public static T[] Array<T>(this ReadOnlyMemory<T> @this)
			=> @this.Segment()?.Array ?? throw new InvalidOperationException("Could not locate array from ReadOnlyMemory");

		public static bool Any<T>(this ReadOnlyMemory<T> @this) => !@this.IsEmpty;
	}
}

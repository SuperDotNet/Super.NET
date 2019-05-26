﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Super.Runtime
{
	public static class Empty<T>
	{
		public static T[] Array { get; } = System.Array.Empty<T>();

		public static ArraySegment<T> Segment { get; } = new ArraySegment<T>(Array);

		public static IEnumerable<T> Enumerable { get; } = System.Linq.Enumerable.Empty<T>();

		public static ImmutableArray<T> Immutable { get; } = ImmutableArray<T>.Empty;
	}
}
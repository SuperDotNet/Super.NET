﻿using System.Collections.Generic;
using Super.Model.Sequences;

namespace Super.Compose.Commands
{
	public sealed class SequenceExtent<T> : Extent<IEnumerable<T>>
	{
		public static SequenceExtent<T> Default { get; } = new SequenceExtent<T>();

		SequenceExtent() {}

		public Extent<T[]> Array => DefaultExtent<T[]>.Default;

		public Extent<Array<T>> Immutable => DefaultExtent<Array<T>>.Default;
	}
}
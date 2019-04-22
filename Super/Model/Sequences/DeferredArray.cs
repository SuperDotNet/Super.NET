﻿using Super.Runtime.Activation;
using System.Collections.Generic;
using System.Linq;

namespace Super.Model.Sequences
{
	public class DeferredArray<T> : IArray<T>, IActivateUsing<IEnumerable<T>>
	{
		readonly IEnumerable<T> _enumerable;

		public DeferredArray(IEnumerable<T> enumerable) => _enumerable = enumerable;

		public Array<T> Get() => _enumerable.ToArray();
	}

	public class DeferredResultArray<T> : IArray<T>
	{
		public Array<T> Get() => default;
	}
}
﻿using System;
using Super.Model.Selection.Stores;
using Super.Runtime;

namespace Super.Model.Selection.Adapters
{
	sealed class Delegates<T> : ReferenceValueStore<ISelect<None, T>, Func<T>>
	{
		public static Delegates<T> Default { get; } = new Delegates<T>();

		Delegates() : base(x => x.Get) {}
	}
}
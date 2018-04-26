﻿using Super.Model.Sources;
using Super.Runtime;
using Super.Runtime.Execution;

namespace Super.Testing.Objects
{
	public sealed class CountingDisposable : DelegatedDisposable, ISource<int>
	{
		readonly ICounter _counter;

		public CountingDisposable() : this(new Counter()) {}

		public CountingDisposable(ICounter counter) : base(counter.Execute) => _counter = counter;

		public int Get() => _counter.Get();
	}
}

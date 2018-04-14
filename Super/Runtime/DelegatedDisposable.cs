using System;

namespace Super.Runtime
{
	sealed class EmptyDisposable : DelegatedDisposable
	{
		public static EmptyDisposable Default { get; } = new EmptyDisposable();

		EmptyDisposable() : base(() => {}) {}
	}

	public class DelegatedDisposable : IDisposable
	{
		readonly Action _callback;

		public DelegatedDisposable(Action callback) => _callback = callback;

		public void Dispose() => _callback();
	}
}
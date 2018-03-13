using System;

namespace Super.Runtime
{
	public class DelegatedDisposable : IDisposable
	{
		readonly Action _callback;

		public DelegatedDisposable(Action callback) => _callback = callback;

		public void Dispose() => _callback();
	}
}
using System;
using Super.Runtime.Activation;

namespace Super.Runtime
{
	public class Disposable : IDisposable, IActivateUsing<Action>
	{
		readonly Action _callback;

		public Disposable(Action callback) => _callback = callback;

		public void Dispose() => _callback();
	}
}
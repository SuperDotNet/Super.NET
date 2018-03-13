using System;
using System.Reactive;

namespace Super.Model.Commands
{
	sealed class InvokeCommand<T> : ICommand<T>
	{
		readonly Func<T, Unit> _delegate;

		public InvokeCommand(Func<T, Unit> @delegate) => _delegate = @delegate;

		public void Execute(T parameter)
		{
			_delegate(parameter);
		}
	}
}
using System;
using Super.Model.Commands;

namespace Super.Model.Sequences
{
	public readonly struct Session<T> : IDisposable
	{
		readonly ICommand<T[]> _command;

		public Session(Store<T> store, ICommand<T[]> command)
		{
			Store    = store;
			_command = command;
		}

		public Store<T> Store { get; }

		public void Dispose()
		{
			_command?.Execute(Store.Instance);
		}
	}
}
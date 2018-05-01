using System;
using System.Reactive;

namespace Super.Model.Commands
{
	public class DelegatedCommand<T> : ICommand<T>, IDelegateAware<T>
	{
		readonly Action<T> _command;

		public DelegatedCommand(Action<T> command) => _command = command;

		public void Execute(T parameter) => _command(parameter);

		public Action<T> Get() => _command;
	}

	public class DelegatedCommand : ICommand<Unit>
	{
		readonly Action _command;

		public DelegatedCommand(Action command) => _command = command;

		public void Execute(Unit _) => _command();
	}
}
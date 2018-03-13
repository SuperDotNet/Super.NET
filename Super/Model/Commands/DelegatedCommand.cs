using System;

namespace Super.Model.Commands
{
	public class DelegatedCommand<T> : ICommand<T>
	{
		readonly Action<T> _command;

		public DelegatedCommand(Action<T> command) => _command = command;

		public void Execute(T parameter) => _command(parameter);
	}
}
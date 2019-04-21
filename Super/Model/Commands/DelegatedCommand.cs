using System;
using Super.Runtime;

namespace Super.Model.Commands
{
	public class DelegatedCommand<T> : ICommand<T>
	{
		readonly Action<T> _command;

		public DelegatedCommand(Action<T> command) => _command = command;

		public void Execute(T parameter) => _command(parameter);
	}

	public class DelegatedCommand : ICommand<None>
	{
		readonly Action _command;

		public DelegatedCommand(Action command) => _command = command;

		public void Execute(None _) => _command();
	}
}
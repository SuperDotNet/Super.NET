using System.Reactive;
using Super.Model.Sources;
using Super.Runtime.Activation;

namespace Super.Model.Commands
{
	sealed class CommandSourceAdapter<T> : ISource<T, Unit>, IActivateMarker<ICommand<T>>
	{
		readonly ICommand<T> _command;

		public CommandSourceAdapter(ICommand<T> command) => _command = command;

		public Unit Get(T parameter)
		{
			_command.Execute(parameter);
			return default;
		}
	}
}
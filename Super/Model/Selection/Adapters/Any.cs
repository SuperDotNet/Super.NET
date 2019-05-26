using Super.Model.Commands;
using Super.Runtime;

namespace Super.Model.Selection.Adapters
{
	sealed class Any : ICommand<object>
	{
		readonly ICommand<None> _command;

		public Any(ICommand<None> command) => _command = command;

		public void Execute(object _)
		{
			_command.Execute();
		}
	}
}
using System;
using Super.Model.Commands;
using Super.Model.Selection;
using Super.Model.Sequences.Collections.Commands;

namespace Super.Model.Sequences.Collections.Groups
{
	sealed class AddGroupElementCommand<T> : ICommand<T>
	{
		readonly ISelect<T, ICommand<T>> _commands;

		public AddGroupElementCommand(IGroupCollection<T> collection, ISelect<T, GroupName> name)
			: this(name.Select(collection).Select(ItemCommands<T>.Default)) {}

		public AddGroupElementCommand(ISelect<T, ICommand<T>> commands) => _commands = commands;

		public void Execute(T parameter)
		{
			var command = _commands.Get(parameter) ??
			              throw new InvalidOperationException($"Could not locate a command from {parameter}.");
			command.Execute(parameter);
		}
	}
}
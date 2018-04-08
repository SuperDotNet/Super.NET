using Super.ExtensionMethods;
using Super.Model.Commands;
using System;
using Super.Model.Selection;

namespace Super.Model.Collections
{
	sealed class AddGroupElementCommand<T> : ICommand<T>
	{
		readonly ISelect<T, ICommand<T>> _commands;

		public AddGroupElementCommand(IGroupCollection<T> collection, ISelect<T, GroupName> name)
			: this(ItemCommands<T>.Default.In(collection.In(name))) {}

		public AddGroupElementCommand(ISelect<T, ICommand<T>> commands) => _commands = commands;

		public void Execute(T parameter)
		{
			var command = _commands.Get(parameter) ??
			              throw new InvalidOperationException($"Could not locate a command from {parameter}.");
			command.Execute(parameter);
		}
	}
}
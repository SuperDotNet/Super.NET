using System;
using Super.ExtensionMethods;
using Super.Model.Commands;
using Super.Model.Sources;

namespace Super.Model.Collections
{
	sealed class AddGroupElementCommand<T> : ICommand<T>
	{
		readonly ISource<T, ICommand<T>> _commands;

		public AddGroupElementCommand(IGroupCollection<T> collection, ISource<T, GroupName> name)
			: this(ItemCommands<T>.Default.In(collection.In(name))) {}

		public AddGroupElementCommand(ISource<T, ICommand<T>> commands) => _commands = commands;

		public void Execute(T parameter)
		{
			var command = _commands.Get(parameter) ??
			              throw new InvalidOperationException($"Could not locate a command from {parameter}.");
			command.Execute(parameter);
		}
	}
}
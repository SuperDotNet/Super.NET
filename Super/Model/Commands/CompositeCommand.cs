using Super.Model.Sequences;
using System;

namespace Super.Model.Commands
{
	public class CompositeCommand<T> : ICommand<T>
	{
		readonly Array<ICommand<T>> _items;

		public CompositeCommand(params ICommand<T>[] items) : this(new Array<ICommand<T>>(items)) {}

		public CompositeCommand(Array<ICommand<T>> items) => _items = items;

		public void Execute(T parameter)
		{
			var length = _items.Length;
			for (var i = 0u; i < length; i++)
			{
				_items[i].Execute(parameter);
			}
		}
	}

	public sealed class ManyCommand<T> : ICommand<Store<T>>
	{
		readonly Action<T> _command;

		public ManyCommand(ICommand<T> command) : this(command.Execute) {}

		public ManyCommand(Action<T> command) => _command = command;

		public void Execute(Store<T> parameter)
		{
			var length = parameter.Length();
			var instance = parameter.Instance;
			for (var i = 0u; i < length; i++)
			{
				_command(instance[i]);
			}
		}
	}
}
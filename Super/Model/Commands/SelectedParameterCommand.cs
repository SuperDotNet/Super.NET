using Super.Runtime.Activation;
using System;
using System.Reactive;

namespace Super.Model.Commands
{
	public class SelectedParameterCommand<TFrom, TTo> : ICommand<TFrom>
	{
		readonly Func<TFrom, TTo> _select;
		readonly Action<TTo>      _source;

		public SelectedParameterCommand(Action<TTo> source, Func<TFrom, TTo> select)
		{
			_select = @select;
			_source  = source;
		}

		public void Execute(TFrom parameter) => _source(_select(parameter));
	}

	public class DelegatedParameterCommand<T> : ICommand
	{
		readonly Action<T> _command;
		readonly Func<T> _parameter;

		public DelegatedParameterCommand(Action<T> command, Func<T> parameter)
		{
			_command = command;
			_parameter = parameter;
		}

		public void Execute(Unit parameter)
		{
			_command(_parameter());
		}
	}

	public interface IAny : ICommand, ICommand<object> {}

	sealed class Any : IAny, IActivateMarker<ICommand<Unit>>
	{
		readonly ICommand<Unit> _command;

		public Any(ICommand<Unit> command) => _command = command;

		public void Execute(object _)
		{
			_command.Execute();
		}

		public void Execute(Unit _)
		{
			_command.Execute();
		}
	}

	sealed class FixedParameterCommand<T> : ICommand
	{
		readonly Action<T> _command;
		readonly T _parameter;

		public FixedParameterCommand(Action<T> command, T parameter)
		{
			_command = command;
			_parameter = parameter;
		}

		public void Execute(Unit parameter)
		{
			_command(_parameter);
		}
	}
}
using Super.ExtensionMethods;
using Super.Model.Sources;
using Super.Runtime.Activation;
using System;
using System.Reactive;

namespace Super.Model.Commands
{
	sealed class InvokeParameterCommand<T> : DecoratedCommand<T>
	{
		public InvokeParameterCommand(Func<T, Unit> @delegate) : base(new InvokeParameterCommand<T, Unit>(@delegate)) {}
	}

	sealed class InvokeParameterCommand<TParameter, TResult>
		: ICommand<TParameter>, IActivateMarker<Func<TParameter, TResult>>
	{
		readonly Func<TParameter, TResult> _delegate;

		public InvokeParameterCommand(Func<TParameter, TResult> @delegate) => _delegate = @delegate;

		public void Execute(TParameter parameter)
		{
			_delegate(parameter);
		}
	}

	sealed class InvokeCommand<T> : ICommand<Unit>
	{
		readonly Func<T> _delegate;

		public InvokeCommand(Func<T> @delegate) => _delegate = @delegate;

		public void Execute(Unit _)
		{
			_delegate();
		}
	}

	class ThrowCommand<T> : ThrowCommand<Unit, T> where T : Exception
	{
		public ThrowCommand(T exception) : base(exception) {}

		public ThrowCommand(ISource<T> source) : base(source) {}

		public ThrowCommand(Func<T> exception) : base(exception) {}
	}

	class ThrowCommand<T, TException> : ICommand<T>, IActivateMarker<TException>,
	                                    IActivateMarker<ISource<TException>>,
	                                    IActivateMarker<Func<TException>>
		where TException : Exception
	{
		readonly Func<TException> _exception;

		public ThrowCommand(TException exception) : this(exception.ToInstance()) {}

		public ThrowCommand(ISource<TException> source) : this(source.ToDelegate()) {}

		public ThrowCommand(Func<TException> exception) => _exception = exception;

		public void Execute(T parameter) => throw _exception();
	}
}
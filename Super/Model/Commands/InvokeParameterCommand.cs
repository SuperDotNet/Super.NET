using Super.Model.Results;
using Super.Runtime;
using Super.Runtime.Activation;
using System;

namespace Super.Model.Commands
{
	sealed class InvokeParameterCommand<T> : DecoratedCommand<T>
	{
		public InvokeParameterCommand(Func<T, None> @delegate) : base(new InvokeParameterCommand<T, None>(@delegate)) {}
	}

	sealed class InvokeParameterCommand<TIn, TOut> : ICommand<TIn>, IActivateUsing<Func<TIn, TOut>>
	{
		readonly Func<TIn, TOut> _delegate;

		public InvokeParameterCommand(Func<TIn, TOut> @delegate) => _delegate = @delegate;

		public void Execute(TIn parameter)
		{
			_delegate(parameter);
		}
	}

	sealed class InvokeCommand<T> : ICommand<None>
	{
		readonly Func<T> _delegate;

		public InvokeCommand(Func<T> @delegate) => _delegate = @delegate;

		public void Execute(None _)
		{
			_delegate();
		}
	}

	class ThrowCommand<T> : ThrowCommand<None, T> where T : Exception
	{
		public ThrowCommand(T exception) : base(exception) {}

		public ThrowCommand(IResult<T> result) : base(result) {}

		public ThrowCommand(Func<T> exception) : base(exception) {}
	}

	class ThrowCommand<T, TException> : ICommand<T>,
	                                    IActivateUsing<TException>,
	                                    IActivateUsing<IResult<TException>>,
	                                    IActivateUsing<Func<TException>>
		where TException : Exception
	{
		readonly Func<TException> _exception;

		public ThrowCommand(TException exception) : this(exception.Start()) {}

		public ThrowCommand(IResult<TException> result) : this(result.Get) {}

		public ThrowCommand(Func<TException> exception) => _exception = exception;

		public void Execute(T parameter) => throw _exception();
	}
}
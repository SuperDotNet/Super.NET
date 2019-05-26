using System;
using Super.Model.Results;
using Super.Runtime;
using Super.Runtime.Activation;

namespace Super.Model.Commands
{
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
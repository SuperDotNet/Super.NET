﻿using Super.ExtensionMethods;
using Super.Model.Instances;
using Super.Runtime.Activation;
using System;
using System.Reactive;

namespace Super.Model.Commands
{
	sealed class InvokeParameterCommand<T> : ICommand<T>
	{
		readonly Func<T, Unit> _delegate;

		public InvokeParameterCommand(Func<T, Unit> @delegate) => _delegate = @delegate;

		public void Execute(T parameter)
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

		public ThrowCommand(IInstance<T> instance) : base(instance) {}

		public ThrowCommand(Func<T> exception) : base(exception) {}
	}

	class ThrowCommand<T, TException> : ICommand<T>, IActivateMarker<TException>,
	                                    IActivateMarker<IInstance<TException>>,
	                                    IActivateMarker<Func<TException>>
		where TException : Exception
	{
		readonly Func<TException> _exception;

		public ThrowCommand(TException exception) : this(exception.ToInstance()) {}

		public ThrowCommand(IInstance<TException> instance) : this(instance.ToDelegate()) {}

		public ThrowCommand(Func<TException> exception) => _exception = exception;

		public void Execute(T parameter) => throw _exception();
	}
}
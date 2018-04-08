using Super.Model.Selection.Stores;
using System;
using System.Reactive;

namespace Super.Model.Commands
{
	sealed class InvokeParameterCommands<T> : ReferenceStore<Func<T, Unit>, ICommand<T>>
	{
		public static InvokeParameterCommands<T> Default { get; } = new InvokeParameterCommands<T>();

		InvokeParameterCommands() : base(x => x.Target as ICommand<T> ?? new InvokeParameterCommand<T>(x)) {}
	}

	sealed class InvokeCommands<T> : ReferenceStore<Func<T>, ICommand<Unit>>
	{
		public static InvokeCommands<T> Default { get; } = new InvokeCommands<T>();

		InvokeCommands() : base(x => x.Target as ICommand<Unit> ?? new InvokeCommand<T>(x)) {}
	}
}
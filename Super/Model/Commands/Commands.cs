using System;
using System.Reactive;
using Super.Model.Sources;

namespace Super.Model.Commands
{
	sealed class Commands<T> : ReferenceStore<Func<T, Unit>, ICommand<T>>
	{
		public static Commands<T> Default { get; } = new Commands<T>();

		Commands() : base(x => x.Target as ICommand<T> ?? new InvokeCommand<T>(x)) {}
	}
}
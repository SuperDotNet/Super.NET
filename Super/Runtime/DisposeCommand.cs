using System;
using Super.Model.Commands;

namespace Super.Runtime
{
	sealed class DisposeCommand : Command<IDisposable>
	{
		public static ICommand<IDisposable> Default { get; } = new DisposeCommand();

		DisposeCommand() : base(x => x.Dispose()) {}
	}
}
using System.Collections.Immutable;
using Super.Model.Commands;

namespace Super.Application.Host.Console {
	public interface IConsoleApplication : ICommand<ImmutableArray<string>> {}
}
using System.Collections.Immutable;
using Super.Model.Commands;

namespace Super.Application.Hosting.Console {
	public interface IConsoleApplication : ICommand<ImmutableArray<string>> {}
}
using Super.Model.Commands;
using Super.Model.Sequences;

namespace Super.Application.Hosting.Console
{
	public interface IConsoleApplication : ICommand<Array<string>> {}
}
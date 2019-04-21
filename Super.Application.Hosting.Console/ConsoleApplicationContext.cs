using Super.Model.Commands;
using Super.Model.Sequences;

namespace Super.Application.Hosting.Console
{
	sealed class ConsoleApplicationContext<T> : ApplicationContext<Array<string>> where T : class, ICommand<Array<string>>
	{
		public ConsoleApplicationContext(T application, IServices services) : base(application, services.ToDisposable()) {}
	}
}

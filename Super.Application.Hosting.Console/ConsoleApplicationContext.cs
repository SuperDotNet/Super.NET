using System.Collections.Immutable;
using Super.Model.Commands;

namespace Super.Application.Hosting.Console
{
	sealed class ConsoleApplicationContext<T> : ApplicationContext<ImmutableArray<string>> where T : class, ICommand<ImmutableArray<string>>
	{
		public ConsoleApplicationContext(T application, IServices services) : base(application, services) {}
	}
}

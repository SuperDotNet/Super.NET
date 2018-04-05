using Super.Model.Commands;
using System.Collections.Immutable;

namespace Super.Application.Console
{
	sealed class ConsoleApplicationContext<T> : ApplicationContext<ImmutableArray<string>> where T : class, ICommand<ImmutableArray<string>>
	{
		public ConsoleApplicationContext(T application, IServices services) : base(application, services) {}
	}
}

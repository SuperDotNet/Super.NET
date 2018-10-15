using Super.Model.Commands;
using System.Collections.Immutable;

namespace Super.Application.Hosting.Console
{
	sealed class ApplicationContexts<T> : ApplicationContexts<ImmutableArray<string>, ConsoleApplicationContext<T>>,
	                                      IApplicationContexts
		where T : class, ICommand<ImmutableArray<string>>
	{
		public static ApplicationContexts<T> Default { get; } = new ApplicationContexts<T>();

		ApplicationContexts() : base(Start.From<string>()
		                                  .AsImmutable()
		                                  .Activate<ConsoleApplicationArgument>()
		                                  .Select(Services<ConsoleApplicationArgument>.Default)) {}
	}
}
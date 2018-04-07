using Super.ExtensionMethods;
using Super.Model.Commands;
using Super.Runtime.Activation;
using System.Collections.Immutable;

namespace Super.Application.Console
{
	sealed class ApplicationContexts<T> : ApplicationContexts<ImmutableArray<string>, ConsoleApplicationContext<T>>,
	                                      IApplicationContexts
		where T : class, ICommand<ImmutableArray<string>>
	{
		public static ApplicationContexts<T> Default { get; } = new ApplicationContexts<T>();

		ApplicationContexts() : base(Activate.New<ImmutableArray<string>, ConsoleApplicationArgument>()
		                                     .Out(Services<ConsoleApplicationArgument>.Default)) {}
	}
}
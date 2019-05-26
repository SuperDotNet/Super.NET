using System.Collections.Immutable;
using Super.Runtime.Activation;

namespace Super.Application.Hosting.Console
{
	sealed class ConsoleApplicationArgument : ApplicationArgument<ImmutableArray<string>>,
	                                          IActivateUsing<ImmutableArray<string>>
	{
		public ConsoleApplicationArgument(ImmutableArray<string> instance) : base(instance) {}
	}
}
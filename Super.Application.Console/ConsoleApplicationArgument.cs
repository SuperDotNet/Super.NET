using System.Collections.Immutable;
using Super.Runtime.Activation;

namespace Super.Application.Console {
	sealed class ConsoleApplicationArgument : ApplicationArgument<ImmutableArray<string>>,
	                                          IActivateMarker<ImmutableArray<string>>
	{
		public ConsoleApplicationArgument(ImmutableArray<string> instance) : base(instance) {}
	}
}
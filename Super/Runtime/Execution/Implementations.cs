using Super.Model.Sources;

namespace Super.Runtime.Execution
{
	static class Implementations
	{
		public static ISource<IContexts> Contexts { get; } = Ambient.For<Contexts>();
	}
}
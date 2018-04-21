using Super.Model.Sources;
using Super.Runtime.Activation;

namespace Super.Runtime.Execution
{
	static class Implementations
	{
		public static ISource<IContexts> Contexts { get; } = Activator<Contexts>.Default.ToAmbient();
	}
}
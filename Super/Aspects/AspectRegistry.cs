using Super.Runtime.Environment;

namespace Super.Aspects
{
	public sealed class AspectRegistry : SystemRegistry<IRegistration>
	{
		public static AspectRegistry Default { get; } = new AspectRegistry();

		AspectRegistry() {}
	}
}
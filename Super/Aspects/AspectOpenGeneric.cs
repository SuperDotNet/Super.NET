using Super.Reflection.Types;

namespace Super.Aspects
{
	public sealed class AspectOpenGeneric : OpenGeneric
	{
		public static AspectOpenGeneric Default { get; } = new AspectOpenGeneric();

		AspectOpenGeneric() : base(typeof(IAspect<,>)) {}
	}
}
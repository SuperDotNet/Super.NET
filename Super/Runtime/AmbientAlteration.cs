using Super.Model.Instances;
using Super.Model.Sources;
using Super.Runtime.Activation;

namespace Super.Runtime
{
	sealed class AmbientAlteration<T> : DecoratedAlteration<IInstance<T>>
	{
		public static AmbientAlteration<T> Default { get; } = new AmbientAlteration<T>();

		AmbientAlteration() : base(Activations<IInstance<T>, Ambient<T>>.Default) {}
	}
}
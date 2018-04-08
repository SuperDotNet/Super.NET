using Super.Model.Selection.Alterations;
using Super.Model.Sources;
using Super.Runtime.Activation;

namespace Super.Runtime
{
	sealed class AmbientAlteration<T> : DecoratedAlteration<ISource<T>>
	{
		public static AmbientAlteration<T> Default { get; } = new AmbientAlteration<T>();

		AmbientAlteration() : base(Activations<ISource<T>, Ambient<T>>.Default) {}
	}
}
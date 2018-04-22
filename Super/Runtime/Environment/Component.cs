using Super.Model.Selection;
using Super.Model.Sources;
using Super.Runtime.Activation;
using Super.Runtime.Execution;

namespace Super.Runtime.Environment
{
	class Component<T> : Ambient<T>, IActivateMarker<T>
	{
		public Component() : this(ComponentLocator<T>.Default) {}

		public Component(IActivator activator) : this(activator, Model.Sources.Default<T>.Instance) {}

		public Component(T fallback) : this(fallback.ToSource()) {}

		public Component(ISource<T> fallback) : this(ComponentLocator<T>.Default, fallback) {}

		public Component(IActivator activator, ISource<T> fallback)
			: base(new FixedActivator<T>(activator.Out(CastOrValue<T>.Default)
			                                      .Or(fallback)
			                                      .Guard(LocateMessage.Default))) {}
	}
}